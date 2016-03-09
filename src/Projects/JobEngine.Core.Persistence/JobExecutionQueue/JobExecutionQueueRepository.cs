using JobEngine.Common;
using JobEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace JobEngine.Core.Persistence
{
    public class JobExecutionQueueRepository : IJobExecutionQueueRepository
    {
        private readonly string connectionString;

        public JobExecutionQueueRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task AckJobRecievedAsync(long jobExecutionQueueId, DateTime dateRecieved)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync(@"UPDATE [JobExecutionQueue]
                               SET [JobExecutionStatus] = @JobExecutionStatus                                 
                                  ,[DateReceivedByClient] = @DateReceivedByClient
                             WHERE JobExecutionQueueId = @JobExecutionQueueId",
                         new
                         {
                             JobExecutionQueueId = jobExecutionQueueId,
                             JobExecutionStatus = JobExecutionStatus.DOWNLOADED_BY_CLIENT,
                             DateReceivedByClient = dateRecieved
                         });
            }
        }

        public async Task<IEnumerable<JobExecutionQueue>> GetJobsToExecuteAsync(Guid jobEngineClientId)
        {
            IEnumerable<JobExecutionQueue> results;
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                results = await conn.QueryAsync<JobExecutionQueue>(@"DECLARE @output TABLE
                                                        (
	                                                    [JobExecutionQueueId] [bigint] NOT NULL,
	                                                    [JobEngineClientId] [uniqueidentifier] NOT NULL,
	                                                    [CustomerId] [uniqueidentifier] NOT NULL,
	                                                    [JobType] [int] NOT NULL,
	                                                    [JobSettings] [nvarchar](max) NULL,
	                                                    [JobExecutionStatus] [int] NOT NULL,
	                                                    [ResultMessage] [nvarchar](max) NULL,
	                                                    [TotalExecutionTimeInMs] [bigint] NULL,
	                                                    [ScheduledJobId] [int] NULL,
	                                                    [CreatedBy] [nvarchar](max) NULL,
	                                                    [DateReceivedByClient] [datetime] NULL,
	                                                    [DateExecutionCompleted] [datetime] NULL,
	                                                    [DateEnteredQueue] [datetime] NULL
                                                        );

                                                    UPDATE [dbo].[JobExecutionQueue]
                                                       SET [JobExecutionStatus] = 1
                                                       OUTPUT inserted.*
                                                       INTO @output
                                                     WHERE JobEngineClientId = @JobEngineClientId AND
                                                           JobExecutionStatus = 0;

                                                    SELECT * FROM @output;",
                                new { JobEngineClientId = jobEngineClientId });
            }
  
            foreach(var job in results)
            {
                if(job.JobType == JobType.AssemblyJob)
                {
                    var deserializedJob = JsonConvert.DeserializeObject<AssemblyJob>(job.JobSettings);
                    foreach(var param in deserializedJob.Parameters)
                    {
                        if(param.IsEncrypted)
                        {
                            if(deserializedJob.Settings.ContainsKey(param.Name))
                            {
                                deserializedJob.Settings[param.Name] =
                                    Encryption.Decrypt(deserializedJob.Settings[param.Name]);
                            }
                        }
                    }
                    job.JobSettings = JsonConvert.SerializeObject(deserializedJob);                   
                }
            }
            return results;
        }

        public async Task UpdateJobExecutionResultAsync(long jobExecutionQueueId, JobExecutionStatus jobExecutionStatus, string resultMessage, DateTime dateCompleted, long totalExecutionTimeInMs)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync(@"
                                DECLARE @scheduledJobId TABLE ( ScheduledJobId INT NULL);
                                
                                UPDATE [JobExecutionQueue]
                                SET  [JobExecutionStatus] = @JobExecutionStatus
                                    ,[ResultMessage] = @ResultMessage
                                    ,[TotalExecutionTimeInMs] = @TotalExecutionTimeInMs                                
                                    ,[DateExecutionCompleted] = @DateExecutionCompleted
                                OUTPUT INSERTED.ScheduledJobId
                                INTO @scheduledJobId
                                WHERE JobExecutionQueueId = @JobExecutionQueueId;
                                
                                IF (SELECT TOP 1 ScheduledJobId FROM @scheduledJobId) IS NOT NULL
                                    BEGIN
                                        UPDATE [ScheduledJobs]
                                        SET  [LastExecutionResult] = @LastExecutionResult
                                            ,[LastExecutionTime] = @LastExecutionTime
                                            ,[TotalExecutionTimeInMs] = @TotalExecutionTimeInMs                                
                                        WHERE ScheduledJobId = (SELECT TOP 1 ScheduledJobId FROM @scheduledJobId)
                                    END;",
                            new
                            {
                                JobExecutionQueueId = jobExecutionQueueId,
                                ResultMessage = resultMessage,
                                TotalExecutionTimeInMs = totalExecutionTimeInMs,
                                DateExecutionCompleted = dateCompleted,
                                JobExecutionStatus = jobExecutionStatus,
                                LastExecutionResult = jobExecutionStatus.ToString(),
                                LastExecutionTime = dateCompleted                                
                            });
            }          
        }

        public async Task UpdateJobExecutionStatusAsync(long jobExecutionQueueId, JobExecutionStatus jobExecutionStatus)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync(@" UPDATE [JobExecutionQueue]
                                SET  [JobExecutionStatus] = @JobExecutionStatus
                                WHERE JobExecutionQueueId = @JobExecutionQueueId;",
                            new
                            {
                                JobExecutionQueueId = jobExecutionQueueId,
                                JobExecutionStatus = jobExecutionStatus
                            });
            }   
        }
        
        public async Task<JobExecutionQueue> GetAsync(long jobExecutionQueueId)
        {
            JobExecutionQueue result;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                var queueItem = await conn.QueryAsync<JobExecutionQueue>(@"
                                SELECT [JobExecutionQueueId]
                                    ,[JobEngineClientId]
                                    ,[CustomerId]
                                    ,[JobType]
                                    ,[JobSettings]
                                    ,[JobExecutionStatus]
                                    ,[ResultMessage]
                                    ,[TotalExecutionTimeInMs]
                                    ,[ScheduledJobId]
                                    ,[CreatedBy]
                                    ,[DateReceivedByClient]
                                    ,[DateExecutionCompleted]
                                    ,[DateEnteredQueue]
                                FROM  [JobExecutionQueue]
                                WHERE JobExecutionQueueId = @JobExecutionQueueId;",
                         new
                         {
                             JobExecutionQueueId = jobExecutionQueueId
                         });
                result = queueItem.Single();                
            }
            return result;
        }

        public async Task<IEnumerable<JobExecutionQueue>> GetScheduledJobsAsync(int scheduledJobId)
        {
            IEnumerable<JobExecutionQueue> results;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                results = await conn.QueryAsync<JobExecutionQueue>(@"
                                SELECT [JobExecutionQueueId]
                                    ,[JobEngineClientId]
                                    ,[CustomerId]
                                    ,[JobType]
                                    ,[JobSettings]
                                    ,[JobExecutionStatus]
                                    ,[ResultMessage]
                                    ,[TotalExecutionTimeInMs]
                                    ,[ScheduledJobId]
                                    ,[CreatedBy]
                                    ,[DateReceivedByClient]
                                    ,[DateExecutionCompleted]
                                    ,[DateEnteredQueue]
                                FROM  [JobExecutionQueue]
                                WHERE ScheduledJobId = @ScheduledJobId
                                ORDER BY JobExecutionQueueId DESC;",
                         new
                         {
                             ScheduledJobId = scheduledJobId
                         });

            }
            return results;
        }

        public async Task<Dictionary<DateTime, int>> GetJobCountTrend(DateTime startDate, DateTime endDate)
        {
            Dictionary<DateTime, int> result = new Dictionary<DateTime, int>();
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                var results = await conn.QueryAsync(@"SELECT FullDate,
	                                   (SELECT COUNT(JobExecutionQueueId)
	                                    FROM JobExecutionQueue
		                                WHERE CAST(FLOOR(CAST(DateExecutionCompleted as float)) as datetime) = FullDate) as Cnt
                                  FROM [DateDimension]
                                  where FullDate >  @StartDate 
                                  AND FullDate < @EndDate
                                  order by FullDate",
                                            new { @StartDate = startDate,@EndDate = endDate });
                foreach (var val in results)
                {
                    result.Add(val.FullDate, val.Cnt);
                }
            }
            return result;
        }

        public async Task<Dictionary<string, int>> GetJobCountGroupedByClient(DateTime startDate, DateTime endDate)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                var results = await conn.QueryAsync(@"SELECT JobEngineClients.Name
	                                    ,COUNT(JobExecutionQueue.JobExecutionQueueId) AS Cnt
                                    FROM [JobExecutionQueue]
                                    INNER JOIN JobEngineClients ON JobEngineClients.JobEngineClientId = JobExecutionQueue.JobEngineClientId
                                    GROUP BY JobEngineClients.NAME",
                                            new { @StartDate = startDate, @EndDate = endDate });
                foreach (var val in results)
                {
                    result.Add(val.Name, val.Cnt);
                }
            }
            return result;
        }
    }
}
