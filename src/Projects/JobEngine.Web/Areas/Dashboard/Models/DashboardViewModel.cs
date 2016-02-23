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
    }
}