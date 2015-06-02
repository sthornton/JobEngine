using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobEngine.Web
{
    public class ClientCommunicator : IClientCommunicator
    {
        public void SendPollRequest(Guid jobEngineClientId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ClientCommunicatorHub>();
            hubContext.Clients.All.sendPollRequest("poll request");
        }
    }
}