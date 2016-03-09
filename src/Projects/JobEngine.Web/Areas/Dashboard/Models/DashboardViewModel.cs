using JobEngine.Web.Areas.Clients.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.Dashboard.Models
{
    public class DashboardViewModel
    {
        public List<JobEngineClientViewModel> JobEngineClients {get;set;}

        public decimal PercSuccessfullScheduleJobs { get; set; }

        public decimal PercClientsOnline { get; set; }

        public Dictionary<DateTime,int> JobCountTrend { get; set; }

        public Dictionary<string,int> JobCountGroupedByClient { get; set; }
    }

    public class ChartValues {
        public string Date { get; set; }
        public int Value { get; set; }
    }

}