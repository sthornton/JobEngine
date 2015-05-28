using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyJobHelper
{
    public interface IPluginMetaData
    {
        string Description { get; }
        string Name { get; }
        string Version { get; }
    }
}
