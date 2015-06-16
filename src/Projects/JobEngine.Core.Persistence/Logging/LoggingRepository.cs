using JobEngine.Models;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public class LoggingRepository : ILoggingRepository
    {
        private readonly string connectionString;

        public LoggingRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task AddJobExecutionLogEntry(JobExecutionLog logEntry)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync(@"INSERT INTO [JobExecutionLogs]
                                    ([JobExecutionQueueId]
                                    ,[Date]
                                    ,[LogLevel]
                                    ,[Logger]
                                    ,[Message]
                                    ,[Exception])
                                VALUES
                                    (@JobExecutionQueueId
                                    ,@Date
                                    ,@LogLevel
                                    ,@Logger
                                    ,@Message
                                    ,@Exception)",
                    new
                    {
                        JobExecutionQueueId = logEntry.JobExecutionQueueId,
                        Date = logEntry.Date,
                        LogLevel = logEntry.LogLevel,
                        Logger = logEntry.Logger,
                        Message = logEntry.Message,
                        Exception = logEntry.Exception
                    });
            }
        }


        public async Task<IEnumerable<JobExecutionLog>> GetLogs(long jobExecutionQueueId)
        {
            IEnumerable<JobExecutionLog> results;
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                results = await conn.QueryAsync<JobExecutionLog>(@"SELECT [JobExecutionLogId]
                                  ,[JobExecutionQueueId]
                                  ,[Date]
                                  ,[LogLevel]
                                  ,[Logger]
                                  ,[Message]
                                  ,[Exception]
                              FROM [JobExecutionLogs]
                              WHERE JobExecutionQueueId = @JobExecutionQueueId
                              ORDER BY Date;",
                        new { JobExecutionQueueId = jobExecutionQueueId });
            }
            return results;
        }
    }
}
