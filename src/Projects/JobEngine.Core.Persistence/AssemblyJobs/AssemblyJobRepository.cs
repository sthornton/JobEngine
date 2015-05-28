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

        public IEnumerable<AssemblyJob> GetAll()
        {
            IEnumerable<AssemblyJob> results;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                results = conn.Query<AssemblyJob>(@"SELECT [AssemblyJobId]
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

        public AssemblyJob Get(int assemblyJobId)
        {
            AssemblyJob results;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                results = conn.Query<AssemblyJob>(@"SELECT [AssemblyJobId]
                                                          ,[Name]
                                                          ,[PluginName]
                                                          ,[PluginFileName]
                                                          ,[PluginFile]
                                                          ,[DateModified]
                                                          ,[ModifiedBy]
                                                          ,[DateCreated]
                                                      FROM [AssemblyJobs]
                                                      WHERE AssemblyJobId = @AssemblyJobId",
                                                new { AssemblyJobId = assemblyJobId }).Single();

                results.Parameters = GetParameters(assemblyJobId).ToList();
            }
            return results;
        }

        public void Edit(AssemblyJob assemblyJob)
        {
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Execute(@"UPDATE [dbo].[AssemblyJobs]
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

        public int Create(AssemblyJob assemblyJob)
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
                result = conn.Query<int>(insert, new
                {                   
                    Name = assemblyJob.Name,          
                    PluginName = assemblyJob.PluginName,
                    PluginFileName = assemblyJob.PluginFileName,
                    PluginFile = assemblyJob.PluginFile,
                    DateModified = DateTime.UtcNow,
                    ModifiedBy = assemblyJob.ModifiedBy,
                    DateCreated = DateTime.UtcNow
                }).Single();
            }
            return result;
        }

        public void Delete(int assemblyJobId)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Execute("DELETE FROM AssemblyJobs WHERE AssemblyJobId = @AssemblyJobId",
                    new { AssemblyJobId = assemblyJobId });
            }
        }

        public int CreateParameter(AssemblyJobParameter assemblyJobParameter)
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
                result = conn.Query<int>(insert, new
                {
                    AssemblyJobId = assemblyJobParameter.AssemblyJobId,
                    Name = assemblyJobParameter.Name,
                    DataType = assemblyJobParameter.DataType,
                    IsRequired = assemblyJobParameter.IsRequired,
                    IsEncrypted = assemblyJobParameter.IsEncrypted,
                    InputValidationRegExPattern = assemblyJobParameter.InputValidationRegExPattern
                }).Single();
            }
            return result;
        }

        public void EditParameter(AssemblyJobParameter assemblyJobParameter)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Execute(@"UPDATE [dbo].[AssemblyJobParameters]
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

        public void DeleteParameter(int assemblyJobParameterId)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Execute(@"DELETE FROM AssemblyJobParameters WHERE AssemblyJobParameterId = @AssemblyJobParameterId",
                            new { AssemblyJobParameterId = assemblyJobParameterId });
            }
        }

        public IEnumerable<AssemblyJobParameter> GetParameters(int assemblyJobId)
        {
            IEnumerable<AssemblyJobParameter> results;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                results = conn.Query<AssemblyJobParameter>(@"SELECT [AssemblyJobParameterId]
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
