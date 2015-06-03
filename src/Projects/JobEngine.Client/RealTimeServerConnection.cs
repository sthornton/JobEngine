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
        private string _serverUrl;
        private string _hubName;
        private Guid _jobEngineClientId;

        public event EventHandler PollRequested;

        public void Connect(string serverUrl, string hubName, Guid jobEngineClientId)
        {
            _serverUrl = serverUrl;
            _hubName = hubName;
            _jobEngineClientId = jobEngineClientId;

            hubConnection = new HubConnection(serverUrl);
            hubProxy = hubConnection.CreateHubProxy(hubName);

            hubConnection.Error -= HubConnection_Error;
            hubConnection.Error += HubConnection_Error;

            hubProxy.On("sendPollRequest", () =>
            {
                // bubble poll requested event
                if (this.PollRequested != null)
                {
                    this.PollRequested(this, new EventArgs());
                }
            });

            hubConnection.Start().Wait();
            hubProxy.Invoke("AwaitingCommands", jobEngineClientId).Wait();
        }

        void HubConnection_Error(Exception obj)
        {
            Connect(_serverUrl, _hubName, _jobEngineClientId);
        }

        public void Disconnect()
        {
            hubConnection.Stop();
            hubConnection.Dispose();
        }
    }
}
