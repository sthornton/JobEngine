using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobEngine.Models
{
    public enum JobExecutionStatus
    {
        NOT_RECIEVED_BY_CLIENT = 0,
        DOWNLOADED_BY_CLIENT = 1,
        EXECUTING = 2,
        SUCCESS = 3,
        WARNING = 4,
        FAILED = 5,
        ERROR = 6,
        FATAL =7,
    }
}
