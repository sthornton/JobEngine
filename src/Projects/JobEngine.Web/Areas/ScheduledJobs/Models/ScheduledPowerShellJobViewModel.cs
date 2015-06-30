using JobEngine.Web.Areas.PowerShellJobs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobEngine.Web.Areas.ScheduledJobs.Models
{
    public class ScheduledPowerShellJobViewModel
    {
        public int ScheduledJobId { get; set; }

        public int PowerShellJobId { get; set; }

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

        public List<PowerShellJobParameterViewModel> PowerShellJobParameters { get; set; }
    }
}