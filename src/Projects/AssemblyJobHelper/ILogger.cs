using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyJobHelper
{
    public interface ILogger
    {
        void Log(long jobExecutionQueueId, LogLevel logLevel, string message, Exception exception = null);
    }
}
