using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobEngine.Web.Areas.AssemblyJobs.Models;
using JobEngine.Web.Areas.Clients.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JobEngine.Web.Areas.ScheduledJobs.Models
{
    public class ScheduledAssemblyJobViewModel
    {
        public int ScheduledJobId { get; set; }

        public int AssemblyJobId { get; set; }

        [Display(Name = "Job Engine Client")]
        public SelectList JobEngineClients { get; set; }

        [Required]
        public Guid SelectedJobEngineClientId { get; set; }

        [Display(Name = "Customer")]
        public SelectList Customers { get; set; }

        [Required]
        public Guid SelectedCustomerId { get; set; }

        [Required, StringLength(150)]
        public string Name { set; get; }

        [Required, StringLength(50), Display(Name = "Cron Schedule")]
        public string CronSchedule { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public List<AssemblyJobParameterViewModel> AssemblyJobParameters { get; set; }

    }
}