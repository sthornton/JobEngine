using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Models
{
    public class JobExecutionLog
    {
        public long JobExecutionLogId { get; set; }

        public long JobExecutionQueueId { get; set; }

        public DateTime Date { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }
    }
}
