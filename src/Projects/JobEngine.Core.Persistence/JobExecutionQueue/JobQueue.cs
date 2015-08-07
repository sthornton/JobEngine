using JobEngine.Core.Persistence;
using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace JobEngine.Persistence
{
    public class JobQueue
    {
        public JobQueue() { }

        public long QueueScheduledJob(ScheduledJob job)
        {
            long jobExecutionQueueId = 0;
            using(SqlConnection conn = new SqlConnection(Settings.JobEngineConnectionString))
            {
                jobExecutionQueueId = conn.Query<long>(@"INSERT INTO [JobExecutionQueue]
                                    ([JobEngineClientId]
                                    ,[CustomerId]
                                    ,[JobType]
                                    ,[JobSettings]
                                    ,[JobExecutionStatus]
                                    ,[ScheduledJobId]
                                    ,[CreatedBy]
                                    ,[DateEnteredQueue])
                                OUTPUT INSERTED.JobExecutionQueueId
                                VALUES
                                    (@JobEngineClientId
                                    ,@CustomerId
                                    ,@JobType
                                    ,@JobSettings
                                    ,@JobExecutionStatus
                                    ,@ScheduledJobId
                                    ,@CreatedBy
                                    ,@DateEnteredQueue)",
                          new
                          {
                              JobEngineClientId = job.JobEngineClientId,
                              CustomerId = job.CustomerId,
                              JobType = job.JobType,
                              JobSettings = job.JobSettings,
                              JobExecutionStatus = JobExecutionStatus.NOT_RECIEVED_BY_CLIENT,
                              ScheduledJobId = job.ScheduledJobId,
                              CreatedBy = "scheduler",
                              DateEnteredQueue = DateTime.UtcNow
                          }).Single();
            }
            return jobExecutionQueueId;
        }

        public long QueueJob(string createdBy, Guid customerId, Guid jobEngineClientId, string jobSettings, JobType jobType)
        {
            long jobExecutionQueueId = 0;
            using (SqlConnection conn = new SqlConnection(Settings.JobEngineConnectionString))
            {
                jobExecutionQueueId = conn.Query<long>(@"INSERT INTO [JobExecutionQueue]
                                    ([JobEngineClientId]
                                    ,[CustomerId]
                                    ,[JobType]
                                    ,[JobSettings]
                                    ,[JobExecutionStatus]
                                    ,[CreatedBy]
                                    ,[DateEnteredQueue])
                                OUTPUT INSERTED.JobExecutionQueueId
                                VALUES
                                    (@JobEngineClientId
                                    ,@CustomerId
                                    ,@JobType
                                    ,@JobSettings
                                    ,@JobExecutionStatus
                                    ,@CreatedBy
                                    ,@DateEnteredQueue)",
                          new
                          {
                              JobEngineClientId =  jobEngineClientId,
                              CustomerId = customerId,
                              JobType = jobType,
                              JobSettings = jobSettings,
                              JobExecutionStatus = JobExecutionStatus.NOT_RECIEVED_BY_CLIENT,
                              CreatedBy = createdBy,
                              DateEnteredQueue = DateTime.UtcNow
                          }).Single();
            }
            return jobExecutionQueueId;           
        }
    }
}