using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.PowerShellJobs.Models
{
    public class PowerShellJobViewModel
    {
        [Key]
        public int PowerShellJobId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Script { get; set; }

        public List<PowerShellJobParameterViewModel> Parameters { get; set; }

        public DateTime DateModified { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime DateCreated { get; set; }
    }
}