using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobEngine.Core.WebApi
{
    public class Settings
    {
        public static int TokenExpirationMinutes { get; set; }

        public static string TempDirectory { get; set; }

        static Settings()
        {
            TokenExpirationMinutes = 1440 * 7;
            TempDirectory = "C:\\Trash";
        }
    }
}