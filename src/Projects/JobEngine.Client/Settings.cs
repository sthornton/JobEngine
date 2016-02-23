using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Client
{
    public static class Settings
    {
        public static string ApiUsername { get; set; }

        public static string ApiPassword { get; set; }

        public static string ApiUrl { get; set; }

        public static string RealTimeUrl { get; set; }

        public static string RealTimeHubName { get; set; }

        public static int PollInterval { get; set; }

        public static Guid JobEngineClientId { get; set; }

        public static string TempFileDirectory { get; set; }

        public static string AssemblyJobPluginsDirectory { get; set; }

        public static string AppDirectory { get; set; }

        static Settings()
        {
            
            ApiUsername = ConfigurationManager.AppSettings["ApiUsername"];            
            ApiPassword = ConfigurationManager.AppSettings["ApiPassword"];            
            ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];
            RealTimeHubName = "ClientCommunicatorHub";
            RealTimeUrl = ConfigurationManager.AppSettings["RealtimeUrl"];
            JobEngineClientId = Guid.Parse(ConfigurationManager.AppSettings["JobEngineClientId"]);
            TempFileDirectory = ConfigurationManager.AppSettings["TempFileDirectory"];
            PollInterval = 20;

            string fullPath = string.Empty;
            if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                AssemblyJobPluginsDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Plugins";
                AppDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            }
            else
            {
                AssemblyJobPluginsDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Plugins";
                AppDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            }
        }
    }
}
