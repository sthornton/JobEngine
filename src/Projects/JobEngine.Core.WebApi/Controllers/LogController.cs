using JobEngine.Core.Persistence;
using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JobEngine.Core.WebApi.Controllers
{
    [Authorize]
    public class LogController : ApiController
    {
        private ILoggingRepository loggingRepository;

        public LogController(ILoggingRepository loggingRepository)
        {
            this.loggingRepository = loggingRepository;
        }

        public void AddJobExecutionLogEntry(JobExecutionLog logEntry)
        {
            this.loggingRepository.AddJobExecutionLogEntry(logEntry);
        }
    }
}
