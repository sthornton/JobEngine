using System.Web.Mvc;

namespace JobEngine.Web.Areas.AssemblyJobs
{
    public class AssemblyJobsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AssemblyJobs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AssemblyJobs_default",
                "AssemblyJobs/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "JobEngine.Web.Areas.AssemblyJobs.Controllers" }
            );
        }
    }
}