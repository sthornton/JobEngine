using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public interface ILoggingRepository
    {
        void AddJobExecutionLogEntry(JobExecutionLog logEntry);

        IEnumerable<JobExecutionLog> GetLogs(long jobExecutionQueueId);
    }
}
