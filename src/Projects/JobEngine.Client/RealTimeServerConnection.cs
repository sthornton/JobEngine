using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Client
{
    public class RealTimeServerConnection
    {
        private static IHubProxy hubProxy;
        private static HubConnection hubConnection;

        public event EventHandler PollRequested;

        public void Connect(string serverUrl, string hubName, Guid jobEngineClientId)
        {
            hubConnection = new HubConnection(serverUrl);
            hubProxy = hubConnection.CreateHubProxy(hubName);


            hubProxy.On<string>("sendPollRequest", (message) =>
            {
                // fire poll requested event
                if (this.PollRequested != null)
                {
                    this.PollRequested(this, new EventArgs());
                }
            });

            hubConnection.Start().Wait();
            hubProxy.Invoke("AwaitingCommands", jobEngineClientId).Wait();
        }

        public void Disconnect()
        {
            hubConnection.Stop();
            hubConnection.Dispose();
        }
    }
}
