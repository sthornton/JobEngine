using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Models
{
    public class AssemblyJob
    {
        [Key]
        public int AssemblyJobId { get; set; }

        public string Name { get; set; }

        public List<AssemblyJobParameter> Parameters { get; set; }

        public string PluginName { get; set; }

        public string PluginFileName { get; set; }

        public byte[] PluginFile { get; set; }

        public Dictionary<string,string> Settings { get; set; }

        public DateTime DateModified { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
