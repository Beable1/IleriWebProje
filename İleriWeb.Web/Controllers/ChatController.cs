using Grpc.Core;
using Grpc.Net.Client;
using IleriWeb.Web.Hubs;
using IleriWeb.Web.Services;
using Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Channels;




namespace IleriWeb.Web.Controllers
{

	public class ChatController : Controller
	{
		private readonly GrpcChatService _grpcChatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(GrpcChatService grpcChatService, IHubContext<ChatHub> hubContext)
        {
            _grpcChatService = grpcChatService;
            _hubContext = hubContext;
        }

        // Anasayfa
        public async Task<IActionResult> Index()
        {
            // gRPC servisi ile mesajları al
            var messages = await _grpcChatService.ListenForMessagesAsync("userdotnet");

            // Mesajları View'a gönder
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Content))
            {
                return BadRequest(new { error = "Message content cannot be empty." });
            }

            // gRPC ile mesaj gönderimi
            await _grpcChatService.SendMessageAsync("userdotnet", "userpython", request.Content);
            
            
            // Basit bir başarı cevabı dönüyoruz
            return Ok(new { message = "Mesaj başarıyla gönderildi." });
        }

        [HttpPost("start-listening")]
        public async Task<IActionResult> StartListening()
        {
            await _grpcChatService.ListenForMessagesAsync("userpython");
            return Ok("Listening started!");
        }

    }

}
