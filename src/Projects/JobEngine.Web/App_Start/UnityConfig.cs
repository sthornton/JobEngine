using System;
using Microsoft.Practices.Unity;
using JobEngine.Core.Persistence;
using System.Configuration;
using JobEngine.Common;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;
using JobEngine.Web.Areas.SiteManagement.Controllers;

namespace JobEngine.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            var unityHubActivator = new UnityHubActivator(container);
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => unityHubActivator);

            var jobEngineConnectionString = ConfigurationManager.ConnectionStrings["JobEngineConnectionString"].ConnectionString;
            var jobSchedulerConnectionString = ConfigurationManager.ConnectionStrings["HangfireConnectionString"].ConnectionString;

            container.RegisterType<IAssemblyJobRepository, AssemblyJobRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<IScheduledJobsRepository, ScheduledJobsRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<ICustomerRepository, CustomerRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<IClientRepository, ClientRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<ILoggingRepository, LoggingRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<IJobExecutionQueueRepository, JobExecutionQueueRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<IPowerShellJobsRepository, PowerShellJobsRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<IClientInstallFilesRepository, ClientInstallFilesRepository>(new InjectionConstructor(jobEngineConnectionString));
            container.RegisterType<ICacheProvider, MemoryCacheProvider>();
            container.RegisterType<IJobScheduler, HangfireJobScheduler>();
            container.RegisterType<IClientCommunicator, ClientCommunicator>();

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<RolesAdminController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
            container.RegisterType<UsersAdminController>(new InjectionConstructor());
        }
    }
}
