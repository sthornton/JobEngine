﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using JobEngine.Models;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace JobEngine.Core.Persistence
{
    public class PowerShellJobsRepository : IPowerShellJobsRepository
    {
        private string connectionString;

        public PowerShellJobsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<PowerShellJob>> GetAllAsync()
        {
            IEnumerable<PowerShellJob> results;
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                results = await conn.QueryAsync<PowerShellJob>(@"SELECT [PowerShellJobId]
                                                                      ,[Name]
                                                                      ,[Description]
                                                                      ,[Script]
                                                                      ,[PSResultType]
                                                                      ,[OverwriteExistingData]
                                                                      ,[DateModified]
                                                                      ,[ModifiedBy]
                                                                      ,[DateCreated]
                                                                  FROM [PowerShellJobs]", conn);
            }
            return results;
        }

        public async Task<PowerShellJob> GetAsync(int powerShellJobId)
        {
            PowerShellJob result;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                var job = await conn.QueryAsync<PowerShellJob>(@"SELECT [PowerShellJobId]
                                                                      ,[Name]
                                                                      ,[Description]
                                                                      ,[Script]
                                                                      ,[PSResultType]
                                                                      ,[OverwriteExistingData]
                                                                      ,[DateModified]
                                                                      ,[ModifiedBy]
                                                                      ,[DateCreated]
                                                                  FROM [PowerShellJobs]
                                                                  WHERE PowerShellJobId = @PowerShellJobId",
                                                                  new { PowerShellJobId = powerShellJobId });
                result = job.Single();
                var parameters = await GetPowerShellJobParametersAsync(powerShellJobId);
                result.Parameters = parameters.ToList();
            }
            return result;
        }

        public async Task<IEnumerable<PowerShellJobParameter>> GetPowerShellJobParametersAsync(int powerShellJobId)
        {
            IEnumerable<PowerShellJobParameter> result;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                result = await conn.QueryAsync<PowerShellJobParameter>(@"SELECT [PowerShellJobParameterId]
                                                                      ,[PowerShellJobId]
                                                                      ,[Name]
                                                                      ,[DataType]
                                                                      ,[IsRequired]
                                                                      ,[IsEncrypted]
                                                                      ,[InputValidationRegExPattern]
                                                                  FROM [JobEngine].[dbo].[PowerShellJobParameters]
                                                                  WHERE PowerShellJobId = @PowerShellJobId",
                                                                  new { PowerShellJobId = powerShellJobId });
            }
            return result.ToList();
        }

        public async Task EditAsync(PowerShellJob powerShellJob)
        {          
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync(@"UPDATE [PowerShellJobs]
                                           SET [Name] = @Name
                                              ,[Description] = @Description
                                              ,[Script] = @Script
                                              ,[PSResultType] = @PSResultType
                                              ,[OverwriteExistingData] = @OverwriteExistingData
                                              ,[DateModified] = @DateModified
                                              ,[ModifiedBy] = @ModifiedBy
                                              ,[DateCreated] = @DateCreated
                                         WHERE PowerShellJobId = @PowerShellJobId",
                                          new
                                          {
                                              Name = powerShellJob.Name,
                                              Description = powerShellJob.Description,
                                              Script = powerShellJob.Script,
                                              PSResultType = powerShellJob.PSResultType,
                                              OverwriteExistingData = powerShellJob.OverwriteExistingData,
                                              DateModified = powerShellJob.DateModified,
                                              ModifiedBy = powerShellJob.ModifiedBy,
                                              DateCreated = powerShellJob.DateCreated,
                                              PowerShellJobId = powerShellJob.PowerShellJobId
                                          });
            }
        }

        public async Task<int> CreateAsync(PowerShellJob powerShellJob)
        {
            int resultId;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                var result =  await conn.QueryAsync<int>(@"INSERT INTO [PowerShellJobs]
                                                                ([Name]
                                                                ,[Description]
                                                                ,[Script]
                                                                ,[PSResultType]
                                                                ,[OverwriteExistingData]
                                                                ,[DateModified]
                                                                ,[ModifiedBy]
                                                                ,[DateCreated])
                                                            OUTPUT INSERTED.PowerShellJobId
                                                            VALUES
                                                                (@Name
                                                                ,@Description
                                                                ,@Script
                                                                ,@PSResultType
                                                                ,@OverwriteExistingData
                                                                ,@DateModified
                                                                ,@ModifiedBy
                                                                ,@DateCreated)",
                                                          new
                                                          {
                                                              Name = powerShellJob.Name,
                                                              Description = powerShellJob.Description,
                                                              Script = powerShellJob.Script,
                                                              PSResultType = powerShellJob.PSResultType,
                                                              OverwriteExistingData = powerShellJob.OverwriteExistingData,
                                                              DateModified = powerShellJob.DateModified,
                                                              ModifiedBy = powerShellJob.ModifiedBy,
                                                              DateCreated = powerShellJob.DateCreated
                                                          });
                resultId = result.Single();                
            }
            return resultId;
        }

        public async Task DeleteAsync(int powerShellJobId)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync(@"DELETE FROM [PowerShellJobs] WHERE PowerShellJobId = @PowerShellJobId",
                                          new { PowerShellJobId = powerShellJobId });
            }
        }

        public async Task<int> CreateParameterAsync(PowerShellJobParameter powerShellJobParameter)
        {
            int result;
            string insert = @"INSERT INTO [PowerShellJobParameters]
                                   ([PowerShellJobId]
                                   ,[Name]
                                   ,[DataType]
                                   ,[IsRequired]
                                   ,[IsEncrypted]
                                   ,[InputValidationRegExPattern])
                            OUTPUT INSERTED.PowerShellJobParameterId
                             VALUES
                                   (@PowerShellJobId
                                   ,@Name
                                   ,@DataType
                                   ,@IsRequired
                                   ,@IsEncrypted
                                   ,@InputValidationRegExPattern)";
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                var insertedId = await conn.QueryAsync<int>(insert, new
                {
                    PowerShellJobId = powerShellJobParameter.PowerShellJobId,
                    Name = powerShellJobParameter.Name,
                    DataType = powerShellJobParameter.DataType,
                    IsRequired = powerShellJobParameter.IsRequired,
                    IsEncrypted = powerShellJobParameter.IsEncrypted,
                    InputValidationRegExPattern = powerShellJobParameter.InputValidationRegExPattern
                });
                result = insertedId.Single();
            }
            return result;
        }


        public async Task DeleteParameterAsync(int powerShellJobParameterId)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync(@"DELETE FROM [PowerShellJobParameters] WHERE PowerShellJobParameterId = @PowerShellJobParameterId",
                                          new { PowerShellJobParameterId = powerShellJobParameterId });
            }
        }


        public async Task CreatePowerShellJobResult(PowerShellJobResult result, int? scheduledJobId, DateTime dateCompleted)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                 await conn.QueryAsync<int>(@"INSERT INTO [PowerShellJobResults]
                                                                   ([ScheduledJobId]
                                                                   ,[JobExecutionQueueId]
                                                                   ,[Results]
                                                                   ,[Errors]
                                                                   ,[DateCompleted])
                                                             VALUES
                                                                   (@ScheduledJobId
                                                                   ,@JobExecutionQueueId
                                                                   ,@Results
                                                                   ,@Errors
                                                                   ,@DateCompleted)",
                                                          new
                                                          {
                                                              JobExecutionQueueId = result.JobExecutionQueueId,
                                                              ScheduledJobId = scheduledJobId,
                                                              Results = result.Results,
                                                              Errors = JsonConvert.SerializeObject(result.Errors),
                                                              DateCompleted = dateCompleted
                                                          });
            }
        }

        public async Task<PowerShellJobResult> GetPowerShellResult(int powerShellResultsId)
        {
            PowerShellJobResult result;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                var job = await conn.QueryAsync<PowerShellJobResult>(@"SELECT [PowerShellJobResultsId]
                                                                      ,[JobExecutionQueueId]
                                                                      ,[ScheduledJobId]
                                                                      ,[Results]
                                                                      ,[Errors]
                                                                      ,[DateCompleted]
                                                                  FROM [PowerShellJobResults]
                                                                  WHERE PowerShellResultsId = @PowerShellResultsId",
                                                                  new { PowerShellResultsId = powerShellResultsId });
                result = job.Single();
            }
            return result;
        }

        public async Task<PowerShellJobResult> GetPowerShellResultFromExecutionId(int jobExecutionQueueId)
        {
            PowerShellJobResult result = new PowerShellJobResult(); ;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(@"SELECT [PowerShellJobResultsId]
                                                                      ,[JobExecutionQueueId]
                                                                      ,[ScheduledJobId]
                                                                      ,[Results]
                                                                      ,[Errors]
                                                                      ,[DateCompleted]
                                                                  FROM [PowerShellJobResults]
                                                                  WHERE JobExecutionQueueId = @jobExecutionQueueId",
                                                                  conn);
                command.Parameters.AddWithValue("jobExecutionQueueId", jobExecutionQueueId);
                conn.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    result.JobExecutionQueueId = (long)reader["JobExecutionQueueId"];
                    result.ScheduledJobId = (int)reader["ScheduledJobId"];
                    result.Results = reader["Results"] != DBNull.Value ? (string)reader["Results"] : null;
                    result.Errors = reader["Errors"] != DBNull.Value ? JsonConvert.DeserializeObject<List<ErrorInfo>>((string)reader["Errors"]) : null;
                    result.DateCompleted = (DateTime)reader["DateCompleted"];
                }
            }
            return result;
        }
    }
}
