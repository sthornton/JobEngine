using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.ScheduledJobs.Models
{
    public class TriggerNowJobResultsViewModel
    {
        [Key]
        public long JobExecutionQueueId { get; set; }

        public Guid JobEngineClientId { get; set; }

        public Guid CustomerId { get; set; }

        public JobType JobType { get; set; }

        public string JobSettings { get; set; }

        public JobExecutionStatus JobExecutionStatus { get; set; }

        public string ResultMessage { get; set; }

        public long? TotalExecutionTimeInMs { get; set; }

        public int? ScheduledJobId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? DateReceivedByClient { get; set; }

        public DateTime? DateExecutionCompleted { get; set; }

        public DateTime? DateEnteredQueue { get; set; }  
    }
}