using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobEngine.Client
{
    public class RealTimeServerConnection
    {
        private IHubProxy hubProxy;
        private HubConnection hubConnection;
        private string serverUrl;
        private string hubName;
        private Guid jobEngineClientId;
        private bool isStopping = false;

        public event EventHandler PollRequested;

        public void Connect(string serverUrl, string hubName, Guid jobEngineClientId)
        {
            this.serverUrl = serverUrl;
            this.hubName = hubName;
            this.jobEngineClientId = jobEngineClientId;

            while(!isStopping)
            {
                try
                {
                    Thread.Sleep(5000);
                    SetupConnection();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }       

            }
        }

        private void SetupConnection()
        {
            hubConnection = new HubConnection(serverUrl);
            hubProxy = hubConnection.CreateHubProxy(hubName);

            hubConnection.Error -= HubConnection_Error;
            hubConnection.Error += HubConnection_Error;

            hubConnection.Closed -= HubConnection_Closed;
            hubConnection.Closed += HubConnection_Closed;

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

        void HubConnection_Closed()
        {
            Console.WriteLine("Realtime connection closed");
        }

        void HubConnection_Error(Exception obj)
        {
            Console.WriteLine("Realtime connection error. " + obj.Message);
        }

        public void Disconnect()
        {
            isStopping = true;
            hubConnection.Stop();
            hubConnection.Dispose();
        }
    }
}
