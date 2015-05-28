using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyJobHelper
{
    /// <summary>
    ///   Adding a new plugin
    ///  1. Create new class library project
    ///  2. Add reference to the JobEngine.Common dll/project
    ///  3. Add reference to the System.ComponentModel.Composition
    ///  4. Implment the IAutomationJobPlugin interface
    ///  5. Decorate the attributes with Name, Version and Description meta data as shown below
    ///  6. Add a mapping between the from address and the plugin/processor
    ///  7. Done, just drop the dll in the plugins directory and restart the service 
    /// </summary>
    public interface IAssemblyJob
    {
        AssemblyJobResult Execute(Dictionary<string, string> jobSettings, long jobExecutionQueueId, ILogger logger);
    }
}
