using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Messaging;
using System.Threading.Tasks;
using Grpc.Core;
using System.Reflection;
using System.ServiceModel.Channels;

namespace IleriWeb.Web.Services
{
	public class GrpcChatService
	{
		private readonly GrpcChannel _channel;
		private readonly Messaging.Messaging.MessagingClient _client;


		public GrpcChatService()
		{
			_channel = GrpcChannel.ForAddress("http://localhost:50051");
			_client = new Messaging.Messaging.MessagingClient(_channel);
		}

		// Mesajı gönder
		public async Task SendMessageAsync(string sender, string receiver, string content)
		{
			var messageRequest = new MessageRequest
			{
				Sender = sender,
				Receiver = receiver,
				Content = content
            };

            

            var metadata = new Metadata { { "username", messageRequest.Sender } };
            using var call = _client.Chat(metadata);

            

			await call.RequestStream.WriteAsync(messageRequest);
			await call.RequestStream.CompleteAsync();
		}


        public async Task<List<string>> ListenForMessagesAsync(string sender)
        {
            var messageResponse = new MessageResponse
            {
                Sender = sender
            };

            var metadata = new Metadata { { "username", messageResponse.Sender } };
            using var call = _client.Chat(metadata);

            var messages = new List<string>();

            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                // Gelen mesajları listeye ekle
                messages.Add($"{response.Sender}: {response.Content}");
            }

            return messages;
        }



    }
}
