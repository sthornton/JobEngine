using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Common
{
    public class JobExecutionQueueResult
    {
        public long JobExecutionQueueId { get; set; }
        public DateTime DateCompleted { get; set; }
        public long TotalExecutionTimeInMs { get; set; } 
        public string Message { get; set; }
        public Result Result { get; set; }
        public Exception Exception { get; set; }
    }

    public enum Result
    {
        SUCCESS,
        WARNING,
        ERROR,
        FATAL
    }
}
