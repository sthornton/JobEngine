using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using JobEngine.Models;
using System.Data.SqlClient;

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
                                                                ,[DateModified]
                                                                ,[ModifiedBy]
                                                                ,[DateCreated])
                                                            OUTPUT INSERTED.PowerShellJobId
                                                            VALUES
                                                                (@Name
                                                                ,@Description
                                                                ,@Script
                                                                ,@PSResultType
                                                                ,@DateModified
                                                                ,@ModifiedBy
                                                                ,@DateCreated)",
                                                          new
                                                          {
                                                              Name = powerShellJob.Name,
                                                              Description = powerShellJob.Description,
                                                              Script = powerShellJob.Script,
                                                              PSResultType = powerShellJob.PSResultType,
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
    }
}
