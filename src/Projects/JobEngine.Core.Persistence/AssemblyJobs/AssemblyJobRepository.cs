using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace JobEngine.Core.Persistence
{
    public class AssemblyJobRepository : IAssemblyJobRepository
    {
        private readonly string connectionString;

        public AssemblyJobRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<AssemblyJob>> GetAllAsync()
        {
            IEnumerable<AssemblyJob> results;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                results = await conn.QueryAsync<AssemblyJob>(@"SELECT [AssemblyJobId]
                                                          ,[Name]
                                                          ,[PluginName]
                                                          ,[PluginFileName]
                                                          ,[PluginFile]
                                                          ,[DateModified]
                                                          ,[ModifiedBy]
                                                          ,[DateCreated]
                                                      FROM [AssemblyJobs]");
            }
            return results;
        }

        public async Task<AssemblyJob> GetAsync(int assemblyJobId)
        {
            AssemblyJob results;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                var job = await conn.QueryAsync<AssemblyJob>(@"SELECT [AssemblyJobId]
                                                          ,[Name]
                                                          ,[PluginName]
                                                          ,[PluginFileName]
                                                          ,[PluginFile]
                                                          ,[DateModified]
                                                          ,[ModifiedBy]
                                                          ,[DateCreated]
                                                      FROM [AssemblyJobs]
                                                      WHERE AssemblyJobId = @AssemblyJobId",
                                                new { AssemblyJobId = assemblyJobId });
                results = job.Single();
                var parameters = await GetParametersAsync(assemblyJobId);
                results.Parameters = parameters.ToList();
            }
            return results;
        }

        public async Task EditAsync(AssemblyJob assemblyJob)
        {
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync(@"UPDATE [dbo].[AssemblyJobs]
                                SET [Name] = @Name
                                    ,[PluginName] = @PluginName
                                    ,[PluginFileName] = @PluginFileName
                                    ,[PluginFile] = @PluginFile
                                    ,[DateModified] = @DateModified
                                    ,[ModifiedBy] = @ModifiedBy
                                    ,[DateCreated] = @DateCreated
                                WHERE AssemblyJobId = @AssemblyJobId",
                            new
                            {
                                AssemblyJobId = assemblyJob.AssemblyJobId,
                                Name = assemblyJob.Name,
                                PluginName = assemblyJob.PluginName,
                                PluginFileName = assemblyJob.PluginFileName,
                                PluginFile = assemblyJob.PluginFile,
                                DateModified = assemblyJob.DateModified,
                                ModifiedBy = assemblyJob.ModifiedBy,
                                DateCreated = assemblyJob.DateCreated
                            });
            }
        }

        public async Task<int> CreateAsync(AssemblyJob assemblyJob)
        {
            int result;
            string insert = @"INSERT INTO [AssemblyJobs]
                                    ([Name]
                                    ,[PluginName]
                                    ,[PluginFileName]
                                    ,[PluginFile]
                                    ,[DateModified]
                                    ,[ModifiedBy]
                                    ,[DateCreated])
                                OUTPUT INSERTED.AssemblyJobId
                                VALUES
                                    (@Name
                                    ,@PluginName
                                    ,@PluginFileName
                                    ,@PluginFile
                                    ,@DateModified
                                    ,@ModifiedBy
                                    ,@DateCreated)";
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                var insertedId = await conn.QueryAsync<int>(insert, new
                {
                    Name = assemblyJob.Name,
                    PluginName = assemblyJob.PluginName,
                    PluginFileName = assemblyJob.PluginFileName,
                    PluginFile = assemblyJob.PluginFile,
                    DateModified = DateTime.UtcNow,
                    ModifiedBy = assemblyJob.ModifiedBy,
                    DateCreated = DateTime.UtcNow
                });
                result = insertedId.Single();
            }
            return result;
        }

        public async Task DeleteAsync(int assemblyJobId)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync("DELETE FROM AssemblyJobs WHERE AssemblyJobId = @AssemblyJobId",
                    new { AssemblyJobId = assemblyJobId });
            }
        }

        public async Task<int> CreateParameterAsync(AssemblyJobParameter assemblyJobParameter)
        {
            int result;
            string insert = @"INSERT INTO [AssemblyJobParameters]
                                   ([AssemblyJobId]
                                   ,[Name]
                                   ,[DataType]
                                   ,[IsRequired]
                                   ,[IsEncrypted]
                                   ,[InputValidationRegExPattern])
                            OUTPUT INSERTED.AssemblyJobParameterId
                             VALUES
                                   (@AssemblyJobId
                                   ,@Name
                                   ,@DataType
                                   ,@IsRequired
                                   ,@IsEncrypted
                                   ,@InputValidationRegExPattern)";
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                var insertedId = await conn.QueryAsync<int>(insert, new
                {
                    AssemblyJobId = assemblyJobParameter.AssemblyJobId,
                    Name = assemblyJobParameter.Name,
                    DataType = assemblyJobParameter.DataType,
                    IsRequired = assemblyJobParameter.IsRequired,
                    IsEncrypted = assemblyJobParameter.IsEncrypted,
                    InputValidationRegExPattern = assemblyJobParameter.InputValidationRegExPattern
                });
                result = insertedId.Single();
            }
            return result;
        }

        public async Task EditParameterAsync(AssemblyJobParameter assemblyJobParameter)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
               await conn.ExecuteAsync(@"UPDATE [dbo].[AssemblyJobParameters]
                               SET [AssemblyJobId] = @AssemblyJobId
                                  ,[Name] = @Name
                                  ,[DataType] = @DataType
                                  ,[IsRequired] = @IsRequired
                                  ,[IsEncrypted] = @IsEncrypted
                                  ,[InputValidationRegExPattern] = @InputValidationRegExPattern
                             WHERE AssemblyJobParameterId = @AssemblyJobParameterId",
                            new
                            {
                                AssemblyJobParameterId = assemblyJobParameter.AssemblyJobParameterId,
                                AssemblyJobId = assemblyJobParameter.AssemblyJobId,
                                Name = assemblyJobParameter.Name,
                                DataType = assemblyJobParameter.DataType,
                                IsRequired = assemblyJobParameter.IsRequired,
                                IsEncrypted = assemblyJobParameter.IsEncrypted,
                                InputValidationRegExPattern = assemblyJobParameter.InputValidationRegExPattern
                            });
            }
        }

        public async Task DeleteParameterAsync(int assemblyJobParameterId)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                await conn.ExecuteAsync(@"DELETE FROM AssemblyJobParameters WHERE AssemblyJobParameterId = @AssemblyJobParameterId",
                            new { AssemblyJobParameterId = assemblyJobParameterId });
            }
        }

        public async Task<IEnumerable<AssemblyJobParameter>> GetParametersAsync(int assemblyJobId)
        {
            IEnumerable<AssemblyJobParameter> results;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                results = await conn.QueryAsync<AssemblyJobParameter>(@"SELECT [AssemblyJobParameterId]
                                                                  ,[AssemblyJobId]
                                                                  ,[Name]
                                                                  ,[DataType]
                                                                  ,[IsRequired]
                                                                  ,[IsEncrypted]
                                                                  ,[InputValidationRegExPattern]
                                                              FROM [AssemblyJobParameters]
                                                              WHERE AssemblyJobId = @AssemblyJobId",
                                                new { AssemblyJobId = assemblyJobId });
            }
            return results;
        }
    }
}
