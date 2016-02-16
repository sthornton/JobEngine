using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public interface IClientInstallFilesRepository
    {
        Task<IEnumerable<ClientInstallFile>> GetAllAsync();
        Task<ClientInstallFile> GetAsync(int clientInstallFileId);

        /// <summary>
        /// Returns details excluding the actual file
        /// </summary>
        /// <param name="clientInstallFileId"></param>
        /// <returns></returns>
        Task<ClientInstallFile> GetDetailsAsync(int clientInstallFileId);
        Task EditAsync(ClientInstallFile clientInstallFile);
        Task<int> CreateAsync(ClientInstallFile clientInstallFile);
        Task DeleteAsync(int assemblyJobId);      
    }
}
