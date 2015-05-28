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
        void AckJobRecieved(long jobExecutionQueueId, DateTime dateRecieved);

        void UpdateJobExecutionResult(long jobExecutionQueueId, JobExecutionStatus jobExecutionStatus, string resultMessage, DateTime dateCompleted, long totalExecutionTimeInMs);

        void UpdateJobExecutionStatus(long jobExecutionQueueId, JobExecutionStatus jobExecutionStatus);

        IEnumerable<JobExecutionQueue> GetJobsToExecute(Guid jobEngineClientId);

        JobExecutionQueue Get(long jobExecutionQueueId);
    }
}
