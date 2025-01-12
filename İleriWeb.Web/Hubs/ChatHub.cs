using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace IleriWeb.Web.Hubs
{
	public class ChatHub : Hub
	{
		
        public async Task SendMessage(string sender, string content)
        {
            await Clients.All.SendAsync("ReceiveMessage", sender, content);
        }

        // Kullanıcıyı gruba ekle
        public async Task JoinGroup(string receiver)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, receiver);
		}

		// Kullanıcıyı gruptan çıkar
		public async Task LeaveGroup(string receiver)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, receiver);
		}
	}
}
