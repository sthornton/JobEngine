using AutoMapper;
using JobEngine.Models;
using JobEngine.Web.App_Start;
using JobEngine.Web.Areas.AssemblyJobs.Models;
using JobEngine.Web.Areas.Clients.Models;
using JobEngine.Web.Areas.Customers.Models;
using JobEngine.Web.Areas.ScheduledJobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JobEngine.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UnityWebActivator.Start();

            Mapper.CreateMap<AssemblyJobParameter, AssemblyJobParameterViewModel>();
            Mapper.CreateMap<AssemblyJob, AssemblyJobViewModel>();
            Mapper.CreateMap<ScheduledJob, ScheduledJobViewModel>();
            Mapper.CreateMap<Customer, CustomerViewModel>();
            Mapper.CreateMap<JobEngineClient, JobEngineClientViewModel>();
            Mapper.CreateMap<JobExecutionQueue, TriggerNowJobResultsViewModel>();  
        }
    }
}
