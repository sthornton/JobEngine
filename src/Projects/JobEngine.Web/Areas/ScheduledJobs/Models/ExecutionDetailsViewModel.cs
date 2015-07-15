using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace JobEngine.Web.Areas.ScheduledJobs.Models
{
    public class ExecutionDetailsViewModel
    {
        public int ScheduledJobId { get; set; }

        public string ScheduledJobName { get; set; }

        public DataTable DataTableResults { get; set; }

        public string StringResults { get; set; }
    }
}