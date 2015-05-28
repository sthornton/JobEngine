using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace JobEngine.Core.Persistence
{
    public class ScheduledJobsRepository: IScheduledJobsRepository
    {
        private string connectionString;

        public ScheduledJobsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<ScheduledJob> GetAll()
        {
            IEnumerable<ScheduledJob> results;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                results = conn.Query<ScheduledJob>(@"SELECT [ScheduledJobId]
                                                          ,[JobEngineClientId]
                                                          ,[CustomerId]
                                                          ,[Name]
                                                          ,[JobType]
                                                          ,[JobSettings]
                                                          ,[CronSchedule]
                                                          ,[LastExecutionResult]
                                                          ,[LastExecutionTime]
                                                          ,[TotalExecutionTimeInMs]
                                                          ,[IsActive]
                                                          ,[DateCreated]
                                                          ,[CreatedBy]
                                                          ,[DateModified]
                                                          ,[ModifiedBy]
                                                      FROM [ScheduledJobs]");
            }
            return results;            
        }

        public int CreateScheduledJob(ScheduledJob scheduledJob)
        {
            int result;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                result = conn.Query<int>(@"INSERT INTO [dbo].[ScheduledJobs]
                                                ([JobEngineClientId]
                                                ,[CustomerId]
                                                ,[Name]
                                                ,[JobType]
                                                ,[JobSettings]
                                                ,[CronSchedule]
                                                ,[LastExecutionResult]
                                                ,[LastExecutionTime]
                                                ,[TotalExecutionTimeInMs]
                                                ,[IsActive]
                                                ,[DateCreated]
                                                ,[CreatedBy]
                                                ,[DateModified]
                                                ,[ModifiedBy])
                                            OUTPUT INSERTED.ScheduledJobId
                                            VALUES
                                                (@JobEngineClientId
                                                ,@CustomerId
                                                ,@Name
                                                ,@JobType
                                                ,@JobSettings
                                                ,@CronSchedule
                                                ,@LastExecutionResult
                                                ,@LastExecutionTime
                                                ,@TotalExecutionTimeInMs
                                                ,@IsActive
                                                ,@DateCreated
                                                ,@CreatedBy
                                                ,@DateModified
                                                ,@ModifiedBy)",
                             new
                             {
                                 JobEngineClientId = scheduledJob.JobEngineClientId,
                                 CustomerId = scheduledJob.CustomerId,
                                 Name = scheduledJob.Name,
                                 JobType = scheduledJob.JobType,
                                 JobSettings = scheduledJob.JobSettings,
                                 CronSchedule = scheduledJob.CronSchedule,
                                 LastExecutionResult = scheduledJob.LastExecutionResult,
                                 LastExecutionTime = scheduledJob.LastExecutionTime,
                                 TotalExecutionTimeInMs = scheduledJob.TotalExecutionTimeInMs,
                                 IsActive = scheduledJob.IsActive,
                                 DateCreated = scheduledJob.DateCreated,
                                 CreatedBy = scheduledJob.CreatedBy,
                                 DateModified = scheduledJob.DateModified,
                                 ModifiedBy = scheduledJob.ModifiedBy
                             }).Single();
            }
            return result;
        }

        public void UpdateScheduledJob(ScheduledJob scheduledJob)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Execute(@"UPDATE [dbo].[ScheduledJobs]
                                       SET [JobEngineClientId] = @JobEngineClientId
                                          ,[CustomerId] = @CustomerId
                                          ,[Name] = @Name
                                          ,[JobType] = @JobType
                                          ,[JobSettings] = @JobSettings
                                          ,[CronSchedule] = @CronSchedule
                                          ,[LastExecutionResult] = @LastExecutionResult
                                          ,[LastExecutionTime] = @LastExecutionTime
                                          ,[TotalExecutionTimeInMs] = @TotalExecutionTimeInMs
                                          ,[IsActive] = @IsActive
                                          ,[DateCreated] = @DateCreated
                                          ,[CreatedBy] = @CreatedBy
                                          ,[DateModified] = @DateModified
                                          ,[ModifiedBy] = @ModifiedBy
                                     WHERE ScheduledJobId = @ScheduledJobId",
                             new
                             {
                                 ScheduledJobId = scheduledJob.ScheduledJobId,
                                 JobEngineClientId = scheduledJob.JobEngineClientId,
                                 CustomerId = scheduledJob.CustomerId,
                                 Name = scheduledJob.Name,
                                 JobType = scheduledJob.JobType,
                                 JobSettings = scheduledJob.JobSettings,
                                 CronSchedule = scheduledJob.CronSchedule,
                                 LastExecutionResult = scheduledJob.LastExecutionResult,
                                 LastExecutionTime = scheduledJob.LastExecutionTime,
                                 TotalExecutionTimeInMs = scheduledJob.TotalExecutionTimeInMs,
                                 IsActive = scheduledJob.IsActive,
                                 DateCreated = scheduledJob.DateCreated,
                                 CreatedBy = scheduledJob.CreatedBy,
                                 DateModified = scheduledJob.DateModified,
                                 ModifiedBy = scheduledJob.ModifiedBy
                             });
            }       
        }

        public void DeleteScheduledJob(int id)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Execute("DELETE FROM ScheduledJobs WHERE ScheduledJobId = @ScheduledJobId",
                    new { ScheduledJobId = id });
            }
        }

        public ScheduledJob Get(int scheduledJobId)
        {
            ScheduledJob result;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                result = conn.Query<ScheduledJob>(@"SELECT [ScheduledJobId]
                                                          ,[JobEngineClientId]
                                                          ,[CustomerId]
                                                          ,[Name]
                                                          ,[JobType]
                                                          ,[JobSettings]
                                                          ,[CronSchedule]
                                                          ,[LastExecutionResult]
                                                          ,[LastExecutionTime]
                                                          ,[TotalExecutionTimeInMs]
                                                          ,[IsActive]
                                                          ,[DateCreated]
                                                          ,[CreatedBy]
                                                          ,[DateModified]
                                                          ,[ModifiedBy]
                                                      FROM [ScheduledJobs]
                                                      WHERE ScheduledJobId = @ScheduledJobId",
                                            new { ScheduledJobId = scheduledJobId }).Single();
            }
            return result;  
        }
    }
}
