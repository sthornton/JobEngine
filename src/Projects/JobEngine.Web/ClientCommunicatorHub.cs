using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace JobEngine.Web
{
    public class ClientCommunicatorHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendPollRequest(Guid jobEngineClientId)
        {
            Clients.All.sendPollRequest();
        }

        public void AwaitingCommands(Guid jobEngineClientId)
        {
            Console.WriteLine(jobEngineClientId.ToString());
        }
    }
}