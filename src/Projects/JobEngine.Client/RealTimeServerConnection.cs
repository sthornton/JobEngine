using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(RealTimeServerConnection));

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

                    if (hubConnection == null)
                        SetupConnection();
                    else if(hubConnection.State == ConnectionState.Disconnected)
                        SetupConnection();
                }
                catch (Exception e)
                {
                    log.Warn(e.Message);
                }       
            }
        }

        private void SetupConnection()
        {
            log.Info("Establishing real-time connection to server");

            hubConnection = new HubConnection(serverUrl);
            hubProxy = hubConnection.CreateHubProxy(hubName);

            hubConnection.Error -= HubConnection_Error;
            hubConnection.Error += HubConnection_Error;

            hubConnection.Closed -= HubConnection_Closed;
            hubConnection.Closed += HubConnection_Closed;

            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;

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

            log.Info("Successfully established real-time connection to server");
        }

        void HubConnection_Closed()
        {
            log.Warn("Realtime connection has been closed");
        }

        void HubConnection_Error(Exception ex)
        {
            log.Warn("Realtime connection error/s detected.",ex);
        }

        public void Disconnect()
        {
            isStopping = true;
            hubConnection.Stop();
            hubConnection.Dispose();
        }
    }
}
