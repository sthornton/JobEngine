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
        public void AddJobExecutionLogEntry(JobExecutionLog logEntry)
        {
            ILoggingRepository repository = RepositoryFactory.GetLoggingRepository();
            repository.AddJobExecutionLogEntry(logEntry);
        }
    }
}
