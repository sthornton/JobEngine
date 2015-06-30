﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Models
{
    public class PowerShellJobResult
    {               
        public string Results { get; set; }

        public List<ErrorInfo> Errors { get; set; }
    }    

    public class ErrorInfo
    {
        public Exception Exception { get; set; }
        public string ScriptStackTrace { get; set; }
        public string Message { get; set; }
        public string RecommendedAction { get; set; }
    }
}
