using JobEngine.Core.Persistence;
using Microsoft.Practices.Unity;
using System.Configuration;
using System.Web.Http;
using Unity.WebApi;

namespace JobEngine.Core.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            var jobEngineConnectionString = ConfigurationManager.ConnectionStrings["JobEngineConnectionString"].ConnectionString;
            var jobSchedulerConnectionString = ConfigurationManager.ConnectionStrings["HangfireConnectionString"].ConnectionString;

            container.RegisterType<IAssemblyJobRepository, AssemblyJobRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<IScheduledJobsRepository, ScheduledJobsRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<ICustomerRepository, CustomerRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<IClientRepository, ClientRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<ILoggingRepository, LoggingRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<IJobExecutionQueueRepository, JobExecutionQueueRepository>(new InjectionConstructor(jobEngineConnectionString));
        }
    }
}