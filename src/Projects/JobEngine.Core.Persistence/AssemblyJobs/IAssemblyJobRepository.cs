using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public interface IAssemblyJobRepository
    {
        Task<IEnumerable<AssemblyJob>> GetAllAsync();
        Task<AssemblyJob> GetAsync(int assemblyJobId);
        Task EditAsync(AssemblyJob assemblyJob);
        Task<int> CreateAsync(AssemblyJob assemblyJob);
        Task DeleteAsync(int assemblyJobId);

        Task<IEnumerable<AssemblyJobParameter>> GetParametersAsync(int assemblyJobId);
        Task<int> CreateParameterAsync(AssemblyJobParameter assemblyJobParameter);
        Task EditParameterAsync(AssemblyJobParameter assemblyJobParameter);
        Task DeleteParameterAsync(int assemblyobParameterId);
    }
}
