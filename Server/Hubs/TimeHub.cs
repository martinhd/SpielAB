using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlazorSignalRApp.Server.Hubs
{
    public class TimeHub : Hub
    {
        public async Task SendMessage(int time)
        {
            await Clients.All.SendAsync("ReceiveMessage", time);
        }
    }
}