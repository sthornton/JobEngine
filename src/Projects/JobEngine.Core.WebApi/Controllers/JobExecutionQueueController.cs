using JobEngine.Common;
using JobEngine.Core.Persistence;
using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace JobEngine.Core.WebApi.Controllers
{
    [Authorize]
    public class JobExecutionQueueController : ApiController
    {
        private IJobExecutionQueueRepository jobExecutionQueueRepository;
        private IClientRepository clientRepository;

        public JobExecutionQueueController(IJobExecutionQueueRepository jobExecutionQueueRepository,
                                           IClientRepository clientRepository)
        {
            this.jobExecutionQueueRepository = jobExecutionQueueRepository;
            this.clientRepository = clientRepository;
        }
        public async Task<HttpResponseMessage> AckJobRecieved(long jobExecutionQueueId,DateTime dateRecieved)
        {
            await this.jobExecutionQueueRepository.AckJobRecievedAsync(jobExecutionQueueId, dateRecieved);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> GetJobsWaitingToExecute(string id, string ipAddress, string hostName)
        {
            var jobEngineClientId = new Guid(id);
            var clients = await this.clientRepository.GetAllAsync();
            var client = clients.Where(x => x.JobEngineClientId == jobEngineClientId && x.IsEnabled && !x.IsDeleted)
                                .FirstOrDefault();
            if (client == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                response.ReasonPhrase = string.Format("No Client exists with that id {0}", id);
                return response;                
            }

            client.LastConnected = DateTime.UtcNow;
            client.IpAddress = ipAddress;
            client.HostName = hostName;
            await clientRepository.EditAsync(client);
            var jobsToExecute = await this.jobExecutionQueueRepository.GetJobsToExecuteAsync(jobEngineClientId);
            return Request.CreateResponse(HttpStatusCode.OK, jobsToExecute);
        }

        public async Task<HttpResponseMessage> UpdateJobExecutionStatus(long jobExecutionQueueId, string jobExecutionStatus)
        {
            JobExecutionStatus status = JobExecutionStatus.ERROR;
            if (!Enum.TryParse(jobExecutionStatus, out status))
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                response.ReasonPhrase = string.Format("The job execution status is not known about: {0}", jobExecutionStatus);
                return response;
            }

            await this.jobExecutionQueueRepository.UpdateJobExecutionStatusAsync(jobExecutionQueueId, status);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> UpdateJobExecutionResult(JobExecutionQueueResult result)
        {
            JobExecutionStatus status = JobExecutionStatus.ERROR;
            switch (result.Result)
            {
                case Result.ERROR: status = JobExecutionStatus.ERROR;
                    break;
                case Result.FATAL: status = JobExecutionStatus.FATAL;
                    break;
                case Result.SUCCESS: status = JobExecutionStatus.SUCCESS;
                    break;
                case Result.WARNING: status = JobExecutionStatus.WARNING;
                    break;
                default:
                    break;
            }

            await  this.jobExecutionQueueRepository.UpdateJobExecutionResultAsync(
                jobExecutionQueueId: result.JobExecutionQueueId,
                jobExecutionStatus: status,
                resultMessage: result.Message,
                dateCompleted: result.DateCompleted,
                totalExecutionTimeInMs: result.TotalExecutionTimeInMs);

            return Request.CreateResponse(HttpStatusCode.OK);
        }        
    }
}
