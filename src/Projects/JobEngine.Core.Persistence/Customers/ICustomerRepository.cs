using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Get all Customers
        /// </summary>
        /// <returns>All Customers</returns>
        IEnumerable<Customer> GetAll();

        /// <summary>
        /// Get the specified customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Specified customer</returns>
        Customer Get(Guid customerId);

        /// <summary>
        /// Creates new Customer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="createdBy"></param>
        /// <returns>New CustomerId</returns>
        Guid Create(string name, string createdBy);

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="customerId"></param>
        void Delete(Guid customerId);

        /// <summary>
        /// Edit customer
        /// </summary>
        /// <param name="customer"></param>
        void Edit(Customer customer);
    }
}
