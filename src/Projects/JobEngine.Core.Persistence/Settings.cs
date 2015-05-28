using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    internal static class Settings
    {
        public static string JobEngineConnectionString { get; set; }

        static Settings()
        {
            try
            {
                JobEngineConnectionString = ConfigurationManager.ConnectionStrings["JobEngineConnectionString"].ConnectionString;
            }
            catch (Exception)
            {
                // Write to a log somewhere
            }
        }
    }
}
