using AssemblyJobHelper;
using JobEngine.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyJobExecuter
{
    public class AssemblyJobPluginManager : MarshalByRefObject, IAssemblyJobPluginManager
    {
        [ImportMany(typeof(IAssemblyJob))]
        private IEnumerable<Lazy<IAssemblyJob, IPluginMetaData>> m_assemblyJobPlugins;
        private CompositionContainer compositionContainer;

        public IEnumerable<Lazy<IAssemblyJob, IPluginMetaData>>  AssemblyJobPlugins
        { 
            get { return m_assemblyJobPlugins; } 
        }

        public void InitializeAssemblyJobPlugins(string directory)
        {
            if(!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            //By using a catalog it allows us to add parts from multiple locations
            var catalog = new AggregateCatalog();

            //Lets add all the parts from this assebly first
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(JobEngineClient).Assembly));

            //Now lets load the parts from the plugin directory
            catalog.Catalogs.Add(new DirectoryCatalog(directory));

            //Now we can add the parts from the catalog to the compisition container
            compositionContainer = new CompositionContainer(catalog);

            //Ok lets fill the imports finally and so we can iterate over em later
            try
            {
                this.compositionContainer.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                throw;
            }
        }
        
        public bool IsPluginLoaded(string name)
        {
            foreach (Lazy<IAssemblyJob, IPluginMetaData> plugin in m_assemblyJobPlugins)
            {
                if (plugin.Metadata.Name.ToLower() == name.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public AssemblyJobHelper.AssemblyJobResult Execute(long jobExecutionQueueId, string pluginName, Dictionary<string, string> settings, ILogger logger)
        {
            AssemblyJobResult result = new AssemblyJobResult();
            foreach (Lazy<IAssemblyJob, IPluginMetaData> plugin in m_assemblyJobPlugins)
            {
                if (plugin.Metadata.Name.ToLower() == pluginName.ToLower())
                {
                    result = plugin.Value.Execute(settings, jobExecutionQueueId, logger);
                    break;
                }
            }
            return result;
        }
   }
}
