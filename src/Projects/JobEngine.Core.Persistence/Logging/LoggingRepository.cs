using JobEngine.Models;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;

namespace JobEngine.Core.Persistence
{
    public class LoggingRepository : ILoggingRepository
    {
        public void AddJobExecutionLogEntry(JobExecutionLog logEntry)
        {            
            using(SqlConnection conn = new SqlConnection(Settings.JobEngineConnectionString))
            {
                conn.Execute(@"INSERT INTO [JobExecutionLogs]
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


        public IEnumerable<JobExecutionLog> GetLogs(long jobExecutionQueueId)
        {
            IEnumerable<JobExecutionLog> results;
            using(SqlConnection conn = new SqlConnection(Settings.JobEngineConnectionString))
            {
                results = conn.Query<JobExecutionLog>(@"SELECT [JobExecutionLogId]
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
