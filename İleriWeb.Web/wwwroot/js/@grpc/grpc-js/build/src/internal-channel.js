"use strict";
/*
 * Copyright 2019 gRPC authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
Object.defineProperty(exports, "__esModule", { value: true });
exports.InternalChannel = void 0;
const channel_credentials_1 = require("./channel-credentials");
const resolving_load_balancer_1 = require("./resolving-load-balancer");
const subchannel_pool_1 = require("./subchannel-pool");
const picker_1 = require("./picker");
const metadata_1 = require("./metadata");
const constants_1 = require("./constants");
const filter_stack_1 = require("./filter-stack");
const compression_filter_1 = require("./compression-filter");
const resolver_1 = require("./resolver");
const logging_1 = require("./logging");
const http_proxy_1 = require("./http_proxy");
const uri_parser_1 = require("./uri-parser");
const connectivity_state_1 = require("./connectivity-state");
const channelz_1 = require("./channelz");
const load_balancing_call_1 = require("./load-balancing-call");
const deadline_1 = require("./deadline");
const resolving_call_1 = require("./resolving-call");
const call_number_1 = require("./call-number");
const control_plane_status_1 = require("./control-plane-status");
const retrying_call_1 = require("./retrying-call");
const subchannel_interface_1 = require("./subchannel-interface");
/**
 * See https://nodejs.org/api/timers.html#timers_setinterval_callback_delay_args
 */
const MAX_TIMEOUT_TIME = 2147483647;
const MIN_IDLE_TIMEOUT_MS = 1000;
// 30 minutes
const DEFAULT_IDLE_TIMEOUT_MS = 30 * 60 * 1000;
const RETRY_THROTTLER_MAP = new Map();
const DEFAULT_RETRY_BUFFER_SIZE_BYTES = 1 << 24; // 16 MB
const DEFAULT_PER_RPC_RETRY_BUFFER_SIZE_BYTES = 1 << 20; // 1 MB
class ChannelSubchannelWrapper extends subchannel_interface_1.BaseSubchannelWrapper {
    constructor(childSubchannel, channel) {
        super(childSubchannel);
        this.channel = channel;
        this.refCount = 0;
        this.subchannelStateListener = (subchannel, previousState, newState, keepaliveTime) => {
            channel.throttleKeepalive(keepaliveTime);
        };
        childSubchannel.addConnectivityStateListener(this.subchannelStateListener);
    }
    ref() {
        this.child.ref();
        this.refCount += 1;
    }
    unref() {
        this.child.unref();
        this.refCount -= 1;
        if (this.refCount <= 0) {
            this.child.removeConnectivityStateListener(this.subchannelStateListener);
            this.channel.removeWrappedSubchannel(this);
        }
    }
}
class ShutdownPicker {
    pick(pickArgs) {
        return {
            pickResultType: picker_1.PickResultType.DROP,
            status: {
                code: constants_1.Status.UNAVAILABLE,
                details: 'Channel closed before call started',
                metadata: new metadata_1.Metadata()
            },
            subchannel: null,
            onCallStarted: null,
            onCallEnded: null
        };
    }
}
class InternalChannel {
    constructor(target, credentials, options) {
        var _a, _b, _c, _d, _e, _f, _g, _h;
        this.credentials = credentials;
        this.options = options;
        this.connectivityState = connectivity_state_1.ConnectivityState.IDLE;
        this.currentPicker = new picker_1.UnavailablePicker();
        /**
         * Calls queued up to get a call config. Should only be populated before the
         * first time the resolver returns a result, which includes the ConfigSelector.
         */
        this.configSelectionQueue = [];
        this.pickQueue = [];
        this.connectivityStateWatchers = [];
        this.configSelector = null;
        /**
         * This is the error from the name resolver if it failed most recently. It
         * is only used to end calls that start while there is no config selector
         * and the name resolver is in backoff, so it should be nulled if
         * configSelector becomes set or the channel state becomes anything other
         * than TRANSIENT_FAILURE.
         */
        this.currentResolutionError = null;
        this.wrappedSubchannels = new Set();
        this.callCount = 0;
        this.idleTimer = null;
        // Channelz info
        this.channelzEnabled = true;
        this.callTracker = new channelz_1.ChannelzCallTracker();
        this.childrenTracker = new channelz_1.ChannelzChildrenTracker();
        /**
         * Randomly generated ID to be passed to the config selector, for use by
         * ring_hash in xDS. An integer distributed approximately uniformly between
         * 0 and MAX_SAFE_INTEGER.
         */
        this.randomChannelId = Math.floor(Math.random() * Number.MAX_SAFE_INTEGER);
        if (typeof target !== 'string') {
            throw new TypeError('Channel target must be a string');
        }
        if (!(credentials instanceof channel_credentials_1.ChannelCredentials)) {
            throw new TypeError('Channel credentials must be a ChannelCredentials object');
        }
        if (options) {
            if (typeof options !== 'object') {
                throw new TypeError('Channel options must be an object');
            }
        }
        this.originalTarget = target;
        const originalTargetUri = (0, uri_parser_1.parseUri)(target);
        if (originalTargetUri === null) {
            throw new Error(`Could not parse target name "${target}"`);
        }
        /* This ensures that the target has a scheme that is registered with the
         * resolver */
        const defaultSchemeMapResult = (0, resolver_1.mapUriDefaultScheme)(originalTargetUri);
        if (defaultSchemeMapResult === null) {
            throw new Error(`Could not find a default scheme for target name "${target}"`);
        }
        this.callRefTimer = setInterval(() => { }, MAX_TIMEOUT_TIME);
        (_b = (_a = this.callRefTimer).unref) === null || _b === void 0 ? void 0 : _b.call(_a);
        if (this.options['grpc.enable_channelz'] === 0) {
            this.channelzEnabled = false;
        }
        this.channelzTrace = new channelz_1.ChannelzTrace();
        this.channelzRef = (0, channelz_1.registerChannelzChannel)(target, () => this.getChannelzInfo(), this.channelzEnabled);
        if (this.channelzEnabled) {
            this.channelzTrace.addTrace('CT_INFO', 'Channel created');
        }
        if (this.options['grpc.default_authority']) {
            this.defaultAuthority = this.options['grpc.default_authority'];
        }
        else {
            this.defaultAuthority = (0, resolver_1.getDefaultAuthority)(defaultSchemeMapResult);
        }
        const proxyMapResult = (0, http_proxy_1.mapProxyName)(defaultSchemeMapResult, options);
        this.target = proxyMapResult.target;
        this.options = Object.assign({}, this.options, proxyMapResult.extraOptions);
        /* The global boolean parameter to getSubchannelPool has the inverse meaning to what
         * the grpc.use_local_subchannel_pool channel option means. */
        this.subchannelPool = (0, subchannel_pool_1.getSubchannelPool)(((_c = options['grpc.use_local_subchannel_pool']) !== null && _c !== void 0 ? _c : 0) === 0);
        this.retryBufferTracker = new retrying_call_1.MessageBufferTracker((_d = options['grpc.retry_buffer_size']) !== null && _d !== void 0 ? _d : DEFAULT_RETRY_BUFFER_SIZE_BYTES, (_e = options['grpc.per_rpc_retry_buffer_size']) !== null && _e !== void 0 ? _e : DEFAULT_PER_RPC_RETRY_BUFFER_SIZE_BYTES);
        this.keepaliveTime = (_f = options['grpc.keepalive_time_ms']) !== null && _f !== void 0 ? _f : -1;
        this.idleTimeoutMs = Math.max((_g = options['grpc.client_idle_timeout_ms']) !== null && _g !== void 0 ? _g : DEFAULT_IDLE_TIMEOUT_MS, MIN_IDLE_TIMEOUT_MS);
        const channelControlHelper = {
            createSubchannel: (subchannelAddress, subchannelArgs, credentialsOverride) => {
                const subchannel = this.subchannelPool.getOrCreateSubchannel(this.target, subchannelAddress, Object.assign({}, this.options, subchannelArgs), credentialsOverride !== null && credentialsOverride !== void 0 ? credentialsOverride : this.credentials);
                subchannel.throttleKeepalive(this.keepaliveTime);
                if (this.channelzEnabled) {
                    this.channelzTrace.addTrace('CT_INFO', 'Created subchannel or used existing subchannel', subchannel.getChannelzRef());
                }
                const wrappedSubchannel = new ChannelSubchannelWrapper(subchannel, this);
                this.wrappedSubchannels.add(wrappedSubchannel);
                return wrappedSubchannel;
            },
            updateState: (connectivityState, picker) => {
                this.currentPicker = picker;
                const queueCopy = this.pickQueue.slice();
                this.pickQueue = [];
                if (queueCopy.length > 0) {
                    this.callRefTimerUnref();
                }
                for (const call of queueCopy) {
                    call.doPick();
                }
                this.updateState(connectivityState);
            },
            requestReresolution: () => {
                // This should never be called.
                throw new Error('Resolving load balancer should never call requestReresolution');
            },
            addChannelzChild: (child) => {
                if (this.channelzEnabled) {
                    this.childrenTracker.refChild(child);
                }
            },
            removeChannelzChild: (child) => {
                if (this.channelzEnabled) {
                    this.childrenTracker.unrefChild(child);
                }
            },
        };
        this.resolvingLoadBalancer = new resolving_load_balancer_1.ResolvingLoadBalancer(this.target, channelControlHelper, credentials, options, (serviceConfig, configSelector) => {
            if (serviceConfig.retryThrottling) {
                RETRY_THROTTLER_MAP.set(this.getTarget(), new retrying_call_1.RetryThrottler(serviceConfig.retryThrottling.maxTokens, serviceConfig.retryThrottling.tokenRatio, RETRY_THROTTLER_MAP.get(this.getTarget())));
            }
            else {
                RETRY_THROTTLER_MAP.delete(this.getTarget());
            }
            if (this.channelzEnabled) {
                this.channelzTrace.addTrace('CT_INFO', 'Address resolution succeeded');
            }
            this.configSelector = configSelector;
            this.currentResolutionError = null;
            /* We process the queue asynchronously to ensure that the corresponding
             * load balancer update has completed. */
            process.nextTick(() => {
                const localQueue = this.configSelectionQueue;
                this.configSelectionQueue = [];
                if (localQueue.length > 0) {
                    this.callRefTimerUnref();
                }
                for (const call of localQueue) {
                    call.getConfig();
                }
            });
        }, status => {
            if (this.channelzEnabled) {
                this.channelzTrace.addTrace('CT_WARNING', 'Address resolution failed with code ' +
                    status.code +
                    ' and details "' +
                    status.details +
                    '"');
            }
            if (this.configSelectionQueue.length > 0) {
                this.trace('Name resolution failed with calls queued for config selection');
            }
            if (this.configSelector === null) {
                this.currentResolutionError = Object.assign(Object.assign({}, (0, control_plane_status_1.restrictControlPlaneStatusCode)(status.code, status.details)), { metadata: status.metadata });
            }
            const localQueue = this.configSelectionQueue;
            this.configSelectionQueue = [];
            if (localQueue.length > 0) {
                this.callRefTimerUnref();
            }
            for (const call of localQueue) {
                call.reportResolverError(status);
            }
        });
        this.filterStackFactory = new filter_stack_1.FilterStackFactory([
            new compression_filter_1.CompressionFilterFactory(this, this.options),
        ]);
        this.trace('Channel constructed with options ' +
            JSON.stringify(options, undefined, 2));
        const error = new Error();
        if ((0, logging_1.isTracerEnabled)('channel_stacktrace')) {
            (0, logging_1.trace)(constants_1.LogVerbosity.DEBUG, 'channel_stacktrace', '(' +
                this.channelzRef.id +
                ') ' +
                'Channel constructed \n' +
                ((_h = error.stack) === null || _h === void 0 ? void 0 : _h.substring(error.stack.indexOf('\n') + 1)));
        }
        this.lastActivityTimestamp = new Date();
    }
    getChannelzInfo() {
        return {
            target: this.originalTarget,
            state: this.connectivityState,
            trace: this.channelzTrace,
            callTracker: this.callTracker,
            children: this.childrenTracker.getChildLists(),
        };
    }
    trace(text, verbosityOverride) {
        (0, logging_1.trace)(verbosityOverride !== null && verbosityOverride !== void 0 ? verbosityOverride : constants_1.LogVerbosity.DEBUG, 'channel', '(' + this.channelzRef.id + ') ' + (0, uri_parser_1.uriToString)(this.target) + ' ' + text);
    }
    callRefTimerRef() {
        var _a, _b, _c, _d;
        // If the hasRef function does not exist, always run the code
        if (!((_b = (_a = this.callRefTimer).hasRef) === null || _b === void 0 ? void 0 : _b.call(_a))) {
            this.trace('callRefTimer.ref | configSelectionQueue.length=' +
                this.configSelectionQueue.length +
                ' pickQueue.length=' +
                this.pickQueue.length);
            (_d = (_c = this.callRefTimer).ref) === null || _d === void 0 ? void 0 : _d.call(_c);
        }
    }
    callRefTimerUnref() {
        var _a, _b;
        // If the hasRef function does not exist, always run the code
        if (!this.callRefTimer.hasRef || this.callRefTimer.hasRef()) {
            this.trace('callRefTimer.unref | configSelectionQueue.length=' +
                this.configSelectionQueue.length +
                ' pickQueue.length=' +
                this.pickQueue.length);
            (_b = (_a = this.callRefTimer).unref) === null || _b === void 0 ? void 0 : _b.call(_a);
        }
    }
    removeConnectivityStateWatcher(watcherObject) {
        const watcherIndex = this.connectivityStateWatchers.findIndex(value => value === watcherObject);
        if (watcherIndex >= 0) {
            this.connectivityStateWatchers.splice(watcherIndex, 1);
        }
    }
    updateState(newState) {
        (0, logging_1.trace)(constants_1.LogVerbosity.DEBUG, 'connectivity_state', '(' +
            this.channelzRef.id +
            ') ' +
            (0, uri_parser_1.uriToString)(this.target) +
            ' ' +
            connectivity_state_1.ConnectivityState[this.connectivityState] +
            ' -> ' +
            connectivity_state_1.ConnectivityState[newState]);
        if (this.channelzEnabled) {
            this.channelzTrace.addTrace('CT_INFO', 'Connectivity state change to ' + connectivity_state_1.ConnectivityState[newState]);
        }
        this.connectivityState = newState;
        const watchersCopy = this.connectivityStateWatchers.slice();
        for (const watcherObject of watchersCopy) {
            if (newState !== watcherObject.currentState) {
                if (watcherObject.timer) {
                    clearTimeout(watcherObject.timer);
                }
                this.removeConnectivityStateWatcher(watcherObject);
                watcherObject.callback();
            }
        }
        if (newState !== connectivity_state_1.ConnectivityState.TRANSIENT_FAILURE) {
            this.currentResolutionError = null;
        }
    }
    throttleKeepalive(newKeepaliveTime) {
        if (newKeepaliveTime > this.keepaliveTime) {
            this.keepaliveTime = newKeepaliveTime;
            for (const wrappedSubchannel of this.wrappedSubchannels) {
                wrappedSubchannel.throttleKeepalive(newKeepaliveTime);
            }
        }
    }
    removeWrappedSubchannel(wrappedSubchannel) {
        this.wrappedSubchannels.delete(wrappedSubchannel);
    }
    doPick(metadata, extraPickInfo) {
        return this.currentPicker.pick({
            metadata: metadata,
            extraPickInfo: extraPickInfo,
        });
    }
    queueCallForPick(call) {
        this.pickQueue.push(call);
        this.callRefTimerRef();
    }
    getConfig(method, metadata) {
        if (this.connectivityState !== connectivity_state_1.ConnectivityState.SHUTDOWN) {
            this.resolvingLoadBalancer.exitIdle();
        }
        if (this.configSelector) {
            return {
                type: 'SUCCESS',
                config: this.configSelector(method, metadata, this.randomChannelId),
            };
        }
        else {
            if (this.currentResolutionError) {
                return {
                    type: 'ERROR',
                    error: this.currentResolutionError,
                };
            }
            else {
                return {
                    type: 'NONE',
                };
            }
        }
    }
    queueCallForConfig(call) {
        this.configSelectionQueue.push(call);
        this.callRefTimerRef();
    }
    enterIdle() {
        this.resolvingLoadBalancer.destroy();
        this.updateState(connectivity_state_1.ConnectivityState.IDLE);
        this.currentPicker = new picker_1.QueuePicker(this.resolvingLoadBalancer);
        if (this.idleTimer) {
            clearTimeout(this.idleTimer);
            this.idleTimer = null;
        }
    }
    startIdleTimeout(timeoutMs) {
        var _a, _b;
        this.idleTimer = setTimeout(() => {
            if (this.callCount > 0) {
                /* If there is currently a call, the channel will not go idle for a
                 * period of at least idleTimeoutMs, so check again after that time.
                 */
                this.startIdleTimeout(this.idleTimeoutMs);
                return;
            }
            const now = new Date();
            const timeSinceLastActivity = now.valueOf() - this.lastActivityTimestamp.valueOf();
            if (timeSinceLastActivity >= this.idleTimeoutMs) {
                this.trace('Idle timer triggered after ' +
                    this.idleTimeoutMs +
                    'ms of inactivity');
                this.enterIdle();
            }
            else {
                /* Whenever the timer fires with the latest activity being too recent,
                 * set the timer again for the time when the time since the last
                 * activity is equal to the timeout. This should result in the timer
                 * firing no more than once every idleTimeoutMs/2 on average. */
                this.startIdleTimeout(this.idleTimeoutMs - timeSinceLastActivity);
            }
        }, timeoutMs);
        (_b = (_a = this.idleTimer).unref) === null || _b === void 0 ? void 0 : _b.call(_a);
    }
    maybeStartIdleTimer() {
        if (this.connectivityState !== connectivity_state_1.ConnectivityState.SHUTDOWN &&
            !this.idleTimer) {
            this.startIdleTimeout(this.idleTimeoutMs);
        }
    }
    onCallStart() {
        if (this.channelzEnabled) {
            this.callTracker.addCallStarted();
        }
        this.callCount += 1;
    }
    onCallEnd(status) {
        if (this.channelzEnabled) {
            if (status.code === constants_1.Status.OK) {
                this.callTracker.addCallSucceeded();
            }
            else {
                this.callTracker.addCallFailed();
            }
        }
        this.callCount -= 1;
        this.lastActivityTimestamp = new Date();
        this.maybeStartIdleTimer();
    }
    createLoadBalancingCall(callConfig, method, host, credentials, deadline) {
        const callNumber = (0, call_number_1.getNextCallNumber)();
        this.trace('createLoadBalancingCall [' + callNumber + '] method="' + method + '"');
        return new load_balancing_call_1.LoadBalancingCall(this, callConfig, method, host, credentials, deadline, callNumber);
    }
    createRetryingCall(callConfig, method, host, credentials, deadline) {
        const callNumber = (0, call_number_1.getNextCallNumber)();
        this.trace('createRetryingCall [' + callNumber + '] method="' + method + '"');
        return new retrying_call_1.RetryingCall(this, callConfig, method, host, credentials, deadline, callNumber, this.retryBufferTracker, RETRY_THROTTLER_MAP.get(this.getTarget()));
    }
    createInnerCall(callConfig, method, host, credentials, deadline) {
        // Create a RetryingCall if retries are enabled
        if (this.options['grpc.enable_retries'] === 0) {
            return this.createLoadBalancingCall(callConfig, method, host, credentials, deadline);
        }
        else {
            return this.createRetryingCall(callConfig, method, host, credentials, deadline);
        }
    }
    createResolvingCall(method, deadline, host, parentCall, propagateFlags) {
        const callNumber = (0, call_number_1.getNextCallNumber)();
        this.trace('createResolvingCall [' +
            callNumber +
            '] method="' +
            method +
            '", deadline=' +
            (0, deadline_1.deadlineToString)(deadline));
        const finalOptions = {
            deadline: deadline,
            flags: propagateFlags !== null && propagateFlags !== void 0 ? propagateFlags : constants_1.Propagate.DEFAULTS,
            host: host !== null && host !== void 0 ? host : this.defaultAuthority,
            parentCall: parentCall,
        };
        const call = new resolving_call_1.ResolvingCall(this, method, finalOptions, this.filterStackFactory.clone(), this.credentials._getCallCredentials(), callNumber);
        this.onCallStart();
        call.addStatusWatcher(status => {
            this.onCallEnd(status);
        });
        return call;
    }
    close() {
        this.resolvingLoadBalancer.destroy();
        this.updateState(connectivity_state_1.ConnectivityState.SHUTDOWN);
        this.currentPicker = new ShutdownPicker();
        for (const call of this.configSelectionQueue) {
            call.cancelWithStatus(constants_1.Status.UNAVAILABLE, 'Channel closed before call started');
        }
        this.configSelectionQueue = [];
        for (const call of this.pickQueue) {
            call.cancelWithStatus(constants_1.Status.UNAVAILABLE, 'Channel closed before call started');
        }
        this.pickQueue = [];
        clearInterval(this.callRefTimer);
        if (this.idleTimer) {
            clearTimeout(this.idleTimer);
        }
        if (this.channelzEnabled) {
            (0, channelz_1.unregisterChannelzRef)(this.channelzRef);
        }
        this.subchannelPool.unrefUnusedSubchannels();
    }
    getTarget() {
        return (0, uri_parser_1.uriToString)(this.target);
    }
    getConnectivityState(tryToConnect) {
        const connectivityState = this.connectivityState;
        if (tryToConnect) {
            this.resolvingLoadBalancer.exitIdle();
            this.lastActivityTimestamp = new Date();
            this.maybeStartIdleTimer();
        }
        return connectivityState;
    }
    watchConnectivityState(currentState, deadline, callback) {
        if (this.connectivityState === connectivity_state_1.ConnectivityState.SHUTDOWN) {
            throw new Error('Channel has been shut down');
        }
        let timer = null;
        if (deadline !== Infinity) {
            const deadlineDate = deadline instanceof Date ? deadline : new Date(deadline);
            const now = new Date();
            if (deadline === -Infinity || deadlineDate <= now) {
                process.nextTick(callback, new Error('Deadline passed without connectivity state change'));
                return;
            }
            timer = setTimeout(() => {
                this.removeConnectivityStateWatcher(watcherObject);
                callback(new Error('Deadline passed without connectivity state change'));
            }, deadlineDate.getTime() - now.getTime());
        }
        const watcherObject = {
            currentState,
            callback,
            timer,
        };
        this.connectivityStateWatchers.push(watcherObject);
    }
    /**
     * Get the channelz reference object for this channel. The returned value is
     * garbage if channelz is disabled for this channel.
     * @returns
     */
    getChannelzRef() {
        return this.channelzRef;
    }
    createCall(method, deadline, host, parentCall, propagateFlags) {
        if (typeof method !== 'string') {
            throw new TypeError('Channel#createCall: method must be a string');
        }
        if (!(typeof deadline === 'number' || deadline instanceof Date)) {
            throw new TypeError('Channel#createCall: deadline must be a number or Date');
        }
        if (this.connectivityState === connectivity_state_1.ConnectivityState.SHUTDOWN) {
            throw new Error('Channel has been shut down');
        }
        return this.createResolvingCall(method, deadline, host, parentCall, propagateFlags);
    }
    getOptions() {
        return this.options;
    }
}
exports.InternalChannel = InternalChannel;
//# sourceMappingURL=internal-channel.js.map