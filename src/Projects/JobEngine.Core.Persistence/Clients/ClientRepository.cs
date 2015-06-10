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
    public class ClientRepository : IClientRepository
    {
        private readonly string connectionString;

        public ClientRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<JobEngineClient>> GetAllAsync()
        {
            IEnumerable<JobEngineClient> results;
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                results = await conn.QueryAsync<JobEngineClient>(@"SELECT [JobEngineClientId]
                                                                ,[CustomerId]
                                                                ,[Name]
                                                                ,[HostName]
                                                                ,[IpAddress]
                                                                ,[Username]
                                                                ,[Password]
                                                                ,[IsEnabled]
                                                                ,[IsDeleted]
                                                                ,[LastConnected]
                                                                ,[DateModified]
                                                                ,[ModifiedBy]
                                                                ,[DateCreated]
                                                            FROM [JobEngineClients]");
            }
            return results;
        }

        public async Task<JobEngineClient> GetAsync(Guid jobEngineClientId)
        {
            JobEngineClient result;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                var client = await conn.QueryAsync<JobEngineClient>(@"SELECT [JobEngineClientId]
                                                                ,[CustomerId]
                                                                ,[Name]
                                                                ,[HostName]
                                                                ,[IpAddress]
                                                                ,[Username]
                                                                ,[Password]
                                                                ,[IsEnabled]
                                                                ,[IsDeleted]
                                                                ,[LastConnected]
                                                                ,[DateModified]
                                                                ,[ModifiedBy]
                                                                ,[DateCreated]
                                                            FROM [JobEngineClients] WHERE JobEngineClientId = @JobEngineClientId",
                                                            new { @JobEngineClientId = jobEngineClientId });
                result = client.First();

            }
            return result;
        }

        public async Task DeleteAsync(Guid jobEngineClientId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.ExecuteAsync(@"DELETE FROM JobEngineClients WHERE JobEngineClientId = @JobEngineClientId",
                        new { @JobEngineClientId = jobEngineClientId });
            }
        }

        public async Task<Guid> CreateAsync(Guid customerId, string name, bool isEnabled, string username, string password, string createdBy)
        {
            Guid result;
            string insert = @"INSERT INTO [JobEngineClients]
                                   ([CustomerId]
                                   ,[Name]                                  
                                   ,[Username]
                                   ,[Password]
                                   ,[IsEnabled]
                                   ,[IsDeleted]
                                   ,[DateModified]
                                   ,[ModifiedBy]
                                   ,[DateCreated])
                             OUTPUT INSERTED.JobEngineClientId
                             VALUES
                                   (@CustomerId
                                   ,@Name                                  
                                   ,@Username
                                   ,@Password
                                   ,@IsEnabled
                                   ,@IsDeleted
                                   ,@DateModified
                                   ,@ModifiedBy
                                   ,@DateCreated);";
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                var insertedId = await conn.QueryAsync<Guid>(insert, new
                {
                    CustomerId = customerId,
                    Name = name,
                    Username = username,
                    Password = password,
                    IsEnabled = true,
                    IsDeleted = false,
                    DateModified = DateTime.UtcNow,
                    ModifiedBy = createdBy,
                    DateCreated = DateTime.UtcNow
                });
                result = insertedId.Single();
            }
            return result;
        }

        public async Task EditAsync(JobEngineClient jobEngineClient)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.ExecuteAsync(@"UPDATE [dbo].[JobEngineClients]
                               SET [CustomerId] = @CustomerId
                                  ,[Name] = @Name
                                  ,[HostName] = @HostName
                                  ,[IpAddress] = @IpAddress
                                  ,[Username] = @Username
                                  ,[Password] = @Password
                                  ,[IsEnabled] = @IsEnabled
                                  ,[IsDeleted] = @IsDeleted
                                  ,[LastConnected] = @LastConnected
                                  ,[DateModified] = @DateModified
                                  ,[ModifiedBy] = @ModifiedBy
                                  ,[DateCreated] = @DateCreated
                             WHERE JobEngineClientId = @JobEngineClientId;",
                        new { JobEngineClientId = jobEngineClient.JobEngineClientId,
                              CustomerId = jobEngineClient.CustomerId,
                              Name = jobEngineClient.Name,
                              HostName = jobEngineClient.HostName,
                              IpAddress = jobEngineClient.IpAddress,
                              Username = jobEngineClient.Username,
                              Password = jobEngineClient.Password,
                              IsEnabled = jobEngineClient.IsEnabled,
                              IsDeleted = jobEngineClient.IsDeleted,
                              LastConnected = jobEngineClient.LastConnected,
                              DateModified = jobEngineClient.DateModified,
                              ModifiedBy = jobEngineClient.ModifiedBy,
                              DateCreated = jobEngineClient.DateCreated });
            }
        }
    }
}
