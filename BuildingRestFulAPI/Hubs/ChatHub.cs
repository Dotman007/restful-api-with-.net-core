using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.SendAsync("Send", message);
        }
    }
}
