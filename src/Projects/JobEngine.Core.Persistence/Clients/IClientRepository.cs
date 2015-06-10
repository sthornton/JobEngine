using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public interface IClientRepository
    {
        /// <summary>
        /// Get all Job Engine Clients
        /// </summary>
        /// <returns>All Clients</returns>
        Task<IEnumerable<JobEngineClient>> GetAllAsync();

        /// <summary>
        /// Gets the specified Job Engine Client
        /// </summary>
        /// <param name="jobEngineClientId"></param>
        /// <returns>Requested Job Engine Client</returns>
        Task<JobEngineClient> GetAsync(Guid jobEngineClientId);

        /// <summary>
        /// Deletes the specified client
        /// </summary>
        /// <param name="jobEngineClientId">Id of the Client to be deleted</param>
        Task DeleteAsync(Guid jobEngineClientId);
        
        /// <summary>
        /// Creates new Job Engine Client
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="isEnabled"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="createdBy"></param>
        /// <returns>Id of the newly created Client></returns>
        Task<Guid> CreateAsync(Guid customerId, string name, bool isEnabled, string username, string password, string createdBy);

        /// <summary>
        /// Updates the client in the database
        /// </summary>
        /// <param name="jobEngineClient"></param>
        Task EditAsync(JobEngineClient jobEngineClient);
        

    }
}
