using System.Threading.Tasks;
using BlazorSignalRApp.Shared;
using Microsoft.AspNetCore.SignalR;


namespace BlazorSignalRApp.Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMessage(GameStatus status)
        {
            await Clients.All.SendAsync("ReceiveMessage", status);
        }
    }
}
