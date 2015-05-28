using AssemblyJobHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Client
{
    public interface IAssemblyJobPluginManager
    {
        IEnumerable<Lazy<IAssemblyJob, IPluginMetaData>> AssemblyJobPlugins { get; }

        void InitializeAssemblyJobPlugins(string directory);

        bool IsPluginLoaded(string name);

        AssemblyJobResult Execute(long jobExecutionQueueId, string pluginName, Dictionary<string, string> settings, ILogger logger);
    }
}
