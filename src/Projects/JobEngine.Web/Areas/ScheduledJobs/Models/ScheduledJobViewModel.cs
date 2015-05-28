using JobEngine.Models;
using JobEngine.Web.Areas.Clients.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.ScheduledJobs.Models
{
    public class ScheduledJobViewModel
    {
        [Key]
        public int ScheduledJobId { get; set; }

        [Display(Name = "Job Engine Client")]
        public string JobEngineClientName { get; set; }

        public Guid JobEngineClientId { get; set; }

        public List<JobEngineClientViewModel> JobEngineClients { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        public Guid CustomerId { get; set; }

        public string Name { set; get; }

        [Display(Name = "Job Type")]
        public JobType JobType { get; set; }

        public string JobSettings { get; set; }

        [Display(Name = "Cron Schedule")]
        public string CronSchedule { get; set; }

        [Display(Name = "Execution Result")]
        public string LastExecutionResult { get; set; }

        [Display(Name = "Last Executed")]
        public DateTime? LastExecutionTime { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }      
    }
}