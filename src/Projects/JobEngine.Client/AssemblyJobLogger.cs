using AssemblyJobHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Client
{
    public class AssemblyJobLogger :  MarshalByRefObject, ILogger
    {
        public void Log(long jobExecutionQueueId, LogLevel logLevel, string message, Exception exception)
        {
            try
            {
                JobEngine.Models.LogLevel level = StatusMapper.FromAssemblyLogLevelToJobEngineLogLevel(logLevel);
                JobEngineApi jobEngineApi = new JobEngineApi(Settings.ApiUrl, Settings.ApiUsername, Settings.ApiPassword);
                jobEngineApi.AddJobExecutionLogEntry(jobExecutionQueueId, DateTime.UtcNow,
                    level, "Logger", message, exception);
            }
            catch (Exception e)
            {
                
                throw;
            }
        }
    }
}
