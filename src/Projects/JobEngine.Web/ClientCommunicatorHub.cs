using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using JobEngine.Common;

namespace JobEngine.Web
{
    public class ClientCommunicatorHub : Hub
    {
        private ICacheProvider cacheProvider = new MemoryCacheProvider();
        private static string CACHE_KEY = "Realtime_Job_Client-";

        public void AwaitingCommands(Guid jobEngineClientId)
        {
            cacheProvider.RemoveCacheItem(CACHE_KEY + jobEngineClientId.ToString());
            cacheProvider.AddCacheItem(CACHE_KEY + jobEngineClientId.ToString(), 60 * 50000, Context.ConnectionId);
        }
    }
}