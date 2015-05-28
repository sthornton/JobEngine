using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Client
{
    public class StatusMapper
    {
        public static JobEngine.Models.JobExecutionStatus FromAssemblyJobResultToJobExecutionResult(AssemblyJobHelper.Result result)
        {
            switch (result)
            {
                case AssemblyJobHelper.Result.SUCCESS:
                    return JobEngine.Models.JobExecutionStatus.SUCCESS;
                case AssemblyJobHelper.Result.WARNING:
                    return JobEngine.Models.JobExecutionStatus.WARNING;
                case AssemblyJobHelper.Result.ERROR:
                    return JobEngine.Models.JobExecutionStatus.ERROR;
                case AssemblyJobHelper.Result.FATAL:
                    return Models.JobExecutionStatus.FATAL;
                default:
                    throw new ArgumentOutOfRangeException(
                        "Unable to map from AssemblyJobResult to JobEngine.Common.Result.  Please add new mapping!");
            }
        }

        public static JobEngine.Models.LogLevel FromAssemblyLogLevelToJobEngineLogLevel(AssemblyJobHelper.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case AssemblyJobHelper.LogLevel.DEBUG:
                    return JobEngine.Models.LogLevel.DEBUG;
                case AssemblyJobHelper.LogLevel.INFO:
                    return JobEngine.Models.LogLevel.INFO;
                case AssemblyJobHelper.LogLevel.WARNING:
                    return JobEngine.Models.LogLevel.WARN;
                case AssemblyJobHelper.LogLevel.ERROR:
                    return JobEngine.Models.LogLevel.ERROR;
                case AssemblyJobHelper.LogLevel.FATAL:
                    return JobEngine.Models.LogLevel.FATAL;
                default:
                    throw new ArgumentOutOfRangeException("logLevel", "Please update the status mapper to include the new log level");
            }
        }
    }
}
