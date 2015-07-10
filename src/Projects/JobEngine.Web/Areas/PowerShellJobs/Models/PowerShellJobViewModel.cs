using JobEngine.Models;
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

        [Display(Name = "PowerShell Result Type")]
        public PSResultType PSResultType { get; set; }

        [Display(Name = "Overwrite Existing Data")]
        public bool? OverwriteExistingData { get; set; }

        public List<PowerShellJobParameterViewModel> Parameters { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

         [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}