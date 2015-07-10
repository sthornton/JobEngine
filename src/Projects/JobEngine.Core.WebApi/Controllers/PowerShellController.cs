using JobEngine.Core.Persistence;
using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JobEngine.Core.WebApi.Controllers
{
    public class PowerShellController : ApiController
    {
        private IPowerShellJobsRepository powerShellJobsRepository;

        public PowerShellController(IPowerShellJobsRepository powerShellJobsRepository)
        {
            this.powerShellJobsRepository = powerShellJobsRepository;
        }

        public async Task<HttpResponseMessage> CreatePowerShellJobResult(PowerShellJobResult powerShellJobResult)
        {
            try
            {
                await this.powerShellJobsRepository.CreatePowerShellJobResult(powerShellJobResult, powerShellJobResult.ScheduledJobId, powerShellJobResult.DateCompleted);
            }
            catch (Exception)
            {
                
                throw;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
