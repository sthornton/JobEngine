using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobEngine.Models;

namespace JobEngine.Web.Areas.ScheduledJobs.Models
{
    public class ViewHistoryViewModel
    {
        public IEnumerable<JobExecutionQueue> JobExecutionQueueItems { get; set; }

        public ScheduledJobViewModel ScheduledJob { get; set; }
    }
}