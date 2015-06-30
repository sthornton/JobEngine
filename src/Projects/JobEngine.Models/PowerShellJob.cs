using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Models
{
    public class PowerShellJob
    {
        [Key]
        public int PowerShellJobId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Script { get; set; }

        public PSResultType PSResultType { get; set; }

        public List<PowerShellJobParameter> Parameters { get; set; }

        public DateTime DateModified { get; set; }

        public string ModifiedBy { get; set; }
       
        public DateTime DateCreated { get; set; }
    }

    
}
