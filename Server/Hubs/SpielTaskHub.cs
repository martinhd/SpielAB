using System.Threading.Tasks;
using BlazorSignalRApp.Shared;
using Microsoft.AspNetCore.SignalR;


namespace BlazorSignalRApp.Server.Hubs
{
    public class SpielTaskHub : Hub
    {
        public async Task SendMessage(SpielTask task)
        {
            await Clients.All.SendAsync("ReceiveMessage", task);
        }
    }
}