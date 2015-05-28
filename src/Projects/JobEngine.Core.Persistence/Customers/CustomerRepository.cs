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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string connectionString;

        public CustomerRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<Customer> GetAll()
        {
            IEnumerable<Customer> results;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                results = conn.Query<Customer>(@"SELECT [CustomerId]
                                                    ,[Name]
                                                    ,[IsDeleted]
                                                    ,[DateModified]
                                                    ,[ModifiedBy]
                                                    ,[DateCreated]
                                                FROM [Customers]");
            }
            return results;
        }

        public Customer Get(Guid customerId)
        {
            Customer result;
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                result = conn.Query<Customer>(@"SELECT [CustomerId]
                                                      ,[Name]
                                                      ,[IsDeleted]
                                                      ,[DateModified]
                                                      ,[ModifiedBy]
                                                      ,[DateCreated]
                                                  FROM [Customers] WHERE CustomerId = @CustomerId",
                                                 new { CustomerId = customerId}).Single();
            }
            return result;
        }

        public Guid Create(string name, string createdBy)
        {
            Guid result;
            string insert = @"INSERT INTO [Customers]
                                    ([Name]
                                    ,[IsDeleted]
                                    ,[DateModified]
                                    ,[ModifiedBy]
                                    ,[DateCreated])
                                OUTPUT INSERTED.CustomerId
                                VALUES
                                    (@Name
                                    ,@IsDeleted
                                    ,@DateModified
                                    ,@ModifiedBy
                                    ,@DateCreated);";
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                result = conn.Query<Guid>(insert, new
                {
                    Name = name,
                    IsDeleted = false,
                    DateModified = DateTime.UtcNow,
                    ModifiedBy = createdBy,
                    DateCreated = DateTime.UtcNow
                }).Single();
            }
            return result;
        }

        public void Delete(Guid customerId)
        {
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Execute(@"DELETE FROM Customers WHERE CustomerId = @CustomerId",
                                new { CustomerId = customerId });
            }
        }

        public void Edit(Customer customer)
        {
            using(SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Execute(@"UPDATE [Customers]
                                SET [Name] = @Name
                                    ,[IsDeleted] = @IsDeleted
                                    ,[DateModified] = @DateModified
                                    ,[ModifiedBy] = @ModifiedBy
                                    ,[DateCreated] = @DateCreated
                                WHERE CustomerId = @CustomerId",
                            new {
                                    CustomerId = customer.CustomerId,
                                    Name = customer.Name,
                                    IsDeleted = customer.IsDeleted,
                                    DateModified = customer.DateModified,
                                    ModifiedBy = customer.ModifiedBy,
                                    DateCreated = customer.DateCreated
                            });
            }
        }
    }
}
