using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.AssemblyJobs.Models
{
    public class AssemblyJobViewModel
    {
        [Key]
        public int AssemblyJobId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, Display(Name = "Plugin Name")]
        public string PluginName { get; set; }

        [Required, Display(Name = "Plugin File Name")]
        public string PluginFileName { get; set; }

        [Display(Name="Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name="Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name="Date Modified")]
        public DateTime DateModified { get; set; }
        
        public HttpPostedFileBase File { get; set; }
   
        public List<AssemblyJobParameterViewModel> AssemblyJobParameters { get; set; }
    }
}