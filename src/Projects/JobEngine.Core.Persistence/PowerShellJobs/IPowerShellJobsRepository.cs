using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public interface IPowerShellJobsRepository
    {
        Task<IEnumerable<PowerShellJob>> GetAllAsync();
        Task<PowerShellJob> GetAsync(int powerShellJobId);
        Task EditAsync(PowerShellJob powerShellJob);
        Task<int> CreateAsync(PowerShellJob powerShellJob);
        Task<int> CreateParameterAsync(PowerShellJobParameter powerShellJobParameter);
        Task DeleteAsync(int powerShellJobId);
        Task DeleteParameterAsync(int powerShellJobParameterId);
    }
}
