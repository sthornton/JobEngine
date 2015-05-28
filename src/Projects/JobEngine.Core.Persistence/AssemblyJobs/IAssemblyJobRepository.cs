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
        IEnumerable<AssemblyJob> GetAll();
        AssemblyJob Get(int assemblyJobId);
        void Edit(AssemblyJob assemblyJob);
        int Create(AssemblyJob assemblyJob);
        void Delete(int assemblyJobId);

        IEnumerable<AssemblyJobParameter> GetParameters(int assemblyJobId);
        int CreateParameter(AssemblyJobParameter assemblyJobParameter);
        void EditParameter(AssemblyJobParameter assemblyJobParameter);
        void DeleteParameter(int assemblyobParameterId);
    }
}
