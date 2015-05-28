using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.ScheduledJobs.Models
{
    public class SelectJobTypeViewModel
    {
       public  List<JobTypeItem> JobTypeItems { get; set; }
    }

    public class JobTypeItem
    {        
        public string Name { get; set; }
        public string ActionLink { get; set; }
    }
}