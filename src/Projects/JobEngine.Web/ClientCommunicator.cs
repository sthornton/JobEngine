using JobEngine.Common;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobEngine.Web
{
    public class ClientCommunicator : IClientCommunicator
    {
        private ICacheProvider cacheProvider = new MemoryCacheProvider();
        private static string CACHE_KEY = "Realtime_Job_Client-";

        public void SendPollRequest(Guid jobEngineClientId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ClientCommunicatorHub>();
            var connectionId = cacheProvider.GetCachedItem<string>(CACHE_KEY + jobEngineClientId.ToString());
            if (connectionId != null)
            {                
                hubContext.Clients.Client(connectionId).sendPollRequest();
            }
            else
            {
                throw new Exception(
                    "Job Engine Client with Id " + jobEngineClientId.ToString() + " is not currently connected");
            }         
            hubContext.Clients.All.sendPollRequest("poll request");
        }
    }
}