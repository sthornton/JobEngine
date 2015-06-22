using System.Web.Mvc;

namespace JobEngine.Web.Areas.PowerShellJobs
{
    public class PowerShellJobsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PowerShellJobs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PowerShellJobs_default",
                "PowerShellJobs/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "JobEngine.Web.Areas.PowerShellJobs.Controllers" }
            );
        }
    }
}