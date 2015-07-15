using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public interface IJobExecutionQueueRepository
    {
        Task AckJobRecievedAsync(long jobExecutionQueueId, DateTime dateRecieved);

        Task UpdateJobExecutionResultAsync(long jobExecutionQueueId, JobExecutionStatus jobExecutionStatus, string resultMessage, DateTime dateCompleted, long totalExecutionTimeInMs);

        Task UpdateJobExecutionStatusAsync(long jobExecutionQueueId, JobExecutionStatus jobExecutionStatus);

        Task<IEnumerable<JobExecutionQueue>> GetJobsToExecuteAsync(Guid jobEngineClientId);

        Task<JobExecutionQueue> GetAsync(long jobExecutionQueueId);

        Task<IEnumerable<JobExecutionQueue>> GetScheduledJobsAsync(int scheduledJobId);
    }
}
