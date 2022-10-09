using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boards.WebApp.SignalR
{
    public class StreamingHub : Hub
    {
        public async Task SendMessage(object data)
        {
            await Clients.All.SendAsync("ReceiveMessage", data);
        }
    }
}
