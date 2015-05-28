using System.Web.Mvc;

namespace JobEngine.Web.Areas.ScheduledJobs
{
    public class ScheduledJobsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ScheduledJobs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ScheduledJobs_default",
                "ScheduledJobs/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "JobEngine.Web.Areas.ScheduledJobs.Controllers" }
            );
        }
    }
}