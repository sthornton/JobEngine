using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyJobHelper
{
    [Serializable]
    public class AssemblyJobResult
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public Result Result { get; set; }
    }
}
