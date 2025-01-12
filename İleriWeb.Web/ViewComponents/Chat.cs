using Grpc.Net.Client;
using IleriWeb.Web.Hubs;
using IleriWeb.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IleriWeb.Web.ViewComponents
{
	public class ChatViewComponent : BaseComponent
	{
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly GrpcChatService _grpcChatService;
        public ChatViewComponent(IHubContext<ChatHub> hubContext, GrpcChatService grpcChatService)
        {
            _hubContext = hubContext;
            _grpcChatService = grpcChatService;
        }

        public IViewComponentResult Invoke()
		{
			// Gerekirse kullanıcı bilgilerini veya mesaj geçmişini burada alabilirsiniz
			return View();
		}

        

    }
}
