using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobEngine.Models;
using Dapper;
using System.Data.SqlClient;

namespace JobEngine.Core.Persistence
{
    public class ClientInstallFilesRepository : IClientInstallFilesRepository
    {
        private readonly string connectionString;

        public ClientInstallFilesRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> CreateAsync(ClientInstallFile clientInstallFile)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                var id = await conn.QueryAsync<int>(@"INSERT INTO [ClientInstallFiles]
                                                    ([Name]
                                                    ,[File]
                                                    ,[Version]
                                                    ,[IsActive]
                                                    ,[DateModified]
                                                    ,[ModifiedBy]
                                                    ,[DateCreated]
                                                    ,[CreatedBy])
                                                OUTPUT INSERTED.ClientInstallFileId
                                                VALUES
                                                    (@Name
                                                    ,@File
                                                    ,@Version
                                                    ,@IsActive
                                                    ,@DateModified
                                                    ,@ModifiedBy
                                                    ,@DateCreated
                                                    ,@CreatedBy)",
                                                    new
                                                    {
                                                        @Name = clientInstallFile.Name,
                                                        @File = clientInstallFile.File,
                                                        @Version = clientInstallFile.Version,
                                                        @IsActive = clientInstallFile.IsActive,
                                                        @DateModified = clientInstallFile.DateModified,
                                                        @ModifiedBy = clientInstallFile.ModifiedBy,
                                                        @DateCreated = clientInstallFile.DateCreated,
                                                        @CreatedBy = clientInstallFile.CreatedBy
                                                    });
                result = id.Single();
            }
            return result;
        }

        public async Task DeleteAsync(int clientInstallFileId)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                await conn.QueryAsync(@"DELETE FROM [ClientInstallFiles]
                                        WHERE ClientInstallFileId = @Id");
            }
        }

        public async Task EditAsync(ClientInstallFile clientInstallFile)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                await conn.QueryAsync(@"UPDATE [ClientInstallFiles]
                                        SET [Name] = @Name
                                            ,[File] = @File
                                            ,[Version] = @Version
                                            ,[IsActive] = @IsActive
                                            ,[DateModified] = @DateModified
                                            ,[ModifiedBy] = @ModifiedBy
                                            ,[DateCreated] = @DateCreated
                                            ,[CreatedBy] = @CreatedBy
                                        WHERE ClientInstallFileId = @ClientInstallFileId",
                                new
                                {
                                    @ClientInstallFileId = clientInstallFile.ClientInstallFileId,
                                    @Name = clientInstallFile.Name,
                                    @File = clientInstallFile.File,
                                    @Version = clientInstallFile.Version,
                                    @IsActive = clientInstallFile.IsActive,
                                    @DateModified = clientInstallFile.DateModified,
                                    @ModifiedBy = clientInstallFile.ModifiedBy,
                                    @DateCreated = clientInstallFile.DateCreated,
                                    @CreatedBy = clientInstallFile.CreatedBy
                                });
            }
        }

        public async Task<ClientInstallFile> GetActiveInstallerAsync()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                var installFile = await conn.QueryAsync<ClientInstallFile>(@"SELECT [ClientInstallFileId]
                                                          ,[Name]
                                                          ,[File]
                                                          ,[Version]
                                                          ,[IsActive]
                                                          ,[DateModified]
                                                          ,[ModifiedBy]
                                                          ,[DateCreated]
                                                          ,[CreatedBy]
                                                      FROM [ClientInstallFiles]
                                                      WHERE IsActive = 1");
                return installFile.Single();
            }
        }

        public async Task<IEnumerable<ClientInstallFile>> GetAllAsync()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                return await conn.QueryAsync<ClientInstallFile>(@"SELECT [ClientInstallFileId]
                                                          ,[Name]
                                                          ,[Version]
                                                          ,[IsActive]
                                                          ,[DateModified]
                                                          ,[ModifiedBy]
                                                          ,[DateCreated]
                                                          ,[CreatedBy]
                                                      FROM [ClientInstallFiles]
                                                      ORDER BY IsActive, Name, Version");
            }
        }

        public async Task<ClientInstallFile> GetAsync(int clientInstallFileId)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                var installFile = await conn.QueryAsync<ClientInstallFile>(@"SELECT [ClientInstallFileId]
                                                          ,[Name]
                                                          ,[File]
                                                          ,[Version]
                                                          ,[IsActive]
                                                          ,[DateModified]
                                                          ,[ModifiedBy]
                                                          ,[DateCreated]
                                                          ,[CreatedBy]
                                                      FROM [ClientInstallFiles]
                                                      WHERE ClientInstallFileId = @ClientInstallFileId",
                                                    new
                                                    {
                                                        @ClientInstallFileId = clientInstallFileId
                                                    });
                return installFile.Single();
            }
        }

        public async Task<ClientInstallFile> GetDetailsAsync(int clientInstallFileId)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                var installFile = await conn.QueryAsync<ClientInstallFile>(@"SELECT [ClientInstallFileId]
                                                          ,[Name]
                                                          ,[Version]
                                                          ,[IsActive]
                                                          ,[DateModified]
                                                          ,[ModifiedBy]
                                                          ,[DateCreated]
                                                          ,[CreatedBy]
                                                      FROM [ClientInstallFiles]
                                                      WHERE ClientInstallFileId = @ClientInstallFileId",
                                                    new
                                                    {
                                                        @ClientInstallFileId = clientInstallFileId
                                                    });
                return installFile.Single();
            }
        }
    }
}
