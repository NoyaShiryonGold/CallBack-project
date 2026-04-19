using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Model;

namespace WcfService
{
    public class ChatHub : Hub
    {
        public void SendMessage(Message message)
        {
            Clients.All.receiveMessage(message);
        }
    }
}
