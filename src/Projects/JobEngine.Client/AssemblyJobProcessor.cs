using JobEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using JobEngine.Models;
using Newtonsoft.Json;
using System.IO;
using AssemblyJobHelper;
using System.Diagnostics;

namespace JobEngine.Client
{
    public class AssemblyJobProcessor
    {
        private static ILog log = LogManager.GetLogger(typeof(AssemblyJobProcessor));

        public AssemblyJobProcessor() 
        {
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            var directoryPath = Path.GetDirectoryName(location);
        }

        public AssemblyJobResult ProcessJob(long jobExecutionQueueId, AssemblyJob assemblyJob, ILogger logger)
        {
            AssemblyJobResult result = new AssemblyJobResult 
            { 
                Result = AssemblyJobHelper.Result.ERROR, 
                Message = "Default Message" 
            };
            AppDomainSetup setup = new AppDomainSetup();
            setup.ShadowCopyFiles = "true";
            AppDomain assemblyJobAppDomain = AppDomain.CreateDomain("AssemblyJob:" + jobExecutionQueueId, null, setup);

            try
            {
                string pluginName = assemblyJob.PluginName;
                log.Debug("Executing Assembly Job (JobExecutionId:" + jobExecutionQueueId + ") using assembly " + assemblyJob.PluginFileName);
                
                string fullPathToDll = Path.Combine(Settings.AssemblyJobPluginsDirectory, "AssemblyJobExecuter.dll");

                var pluginManager = (IAssemblyJobPluginManager)assemblyJobAppDomain
                    .CreateInstanceFromAndUnwrap(assemblyName: fullPathToDll, 
                                                 typeName: "AssemblyJobExecuter.AssemblyJobPluginManager");

                pluginManager.InitializeAssemblyJobPlugins(Settings.AssemblyJobPluginsDirectory);

                if (pluginManager.IsPluginLoaded(assemblyJob.PluginName))
                {
                    result = pluginManager.Execute(jobExecutionQueueId: jobExecutionQueueId,
                                                   pluginName: assemblyJob.PluginName,
                                                   settings: assemblyJob.Settings,
                                                   logger: logger);
                }
                else
                {
                    result.Message = "Plugin not found! PluginName: " + pluginName;
                    result.Result = AssemblyJobHelper.Result.FATAL;
                }

                log.Debug("Finished executing Assembly Job (JobExecutionId:" + jobExecutionQueueId + ")");
                log.Debug("Assembly Job Result (JobExecutionId:" + jobExecutionQueueId + ") = " + result.Result.ToString());
                UnloadAppDomain(assemblyJobAppDomain);
                log.Debug("Finished unloading App Domain for Assembly Job (JobExecutionId:" + jobExecutionQueueId + ")");
            }
            catch (Exception e)
            {        
                log.Warn(e);
                result.Exception = e;
                result.Message = "Exception Message: " + e.Message;
                UnloadAppDomain(assemblyJobAppDomain);
            }
            return result;
        }

        private void UnloadAppDomain(AppDomain domain)
        {
            try
            {
                AppDomain.Unload(domain);
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

    }
}
