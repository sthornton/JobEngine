using System.Web.Mvc;

namespace JobEngine.Web.Areas.Clients
{
    public class ClientsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Clients";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Clients_default",
                "Clients/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "JobEngine.Web.Areas.Clients.Controllers"}
            );
        }
    }
}