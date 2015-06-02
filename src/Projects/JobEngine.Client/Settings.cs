using System;
using System.Collections.Generic;
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
            ApiUsername = "testUser";
            ApiPassword = "password";
            ApiUrl = "http://localhost:64196";

            RealTimeHubName = "ClientCommunicatorHub";
            RealTimeUrl = "http://localhost:63376/";

            JobEngineClientId = new Guid("38AFF521-11FB-E411-827B-E8B1FC46C78C");
            TempFileDirectory = "C:\\Temp\\";

            PollInterval = 20;

            string fullPath = string.Empty;
            if(!AppDomain.CurrentDomain.IsDefaultAppDomain())
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
