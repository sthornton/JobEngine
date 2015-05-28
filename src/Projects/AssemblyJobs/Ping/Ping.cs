using AssemblyJobHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Ping
{
    [Export(typeof(IAssemblyJob)),
     ExportMetadata("Name", "Ping"),
     ExportMetadata("Version", "1.1.0.0"),
     ExportMetadata("Description", "Pings the remote host")]
    public class Ping : IAssemblyJob
    {
        public AssemblyJobResult Execute(Dictionary<string, string> jobSettings, long jobExecutionQueueId, ILogger logger)
        {
            AssemblyJobResult result = new AssemblyJobResult();
         
            if(PingHost(jobSettings["Host"]))
            {
                result.Message = "Up";
                result.Result = Result.SUCCESS;

                try
                {
                    logger.Log(jobExecutionQueueId, LogLevel.INFO, "Host pinged successfully", null);
                    //System.Threading.Thread.Sleep(1000);
                }
                catch (Exception e)
                {                    
                    throw;
                }
            }
            else
            {
                result.Message = "Down";
                result.Result = Result.SUCCESS;
            }
            return result;
        }

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            System.Net.NetworkInformation.Ping pinger = new System.Net.NetworkInformation.Ping();

            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }

            return pingable;
        }
    }
}
