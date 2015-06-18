using Hangfire;
using Hangfire.SqlServer;
using JobEngine.Common;
using JobEngine.Core.Persistence;
using JobEngine.Models;
using JobEngine.Persistence;
using JobEngine.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartup(typeof(JobEngine.Web.Startup))]

namespace JobEngine.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnectionString");
          //  app.UseHangfireServer();
            app.UseHangfireDashboard();
            ConfigureAuth(app);
           // InitializeJobs();            
            app.MapSignalR();
        }

        public void InitializeJobs()
        {
            try
            {
                IScheduledJobsRepository repository = RepositoryFactory.GetScheduledJobsRepository();
                var jobs = repository.GetAll().Result.Where(x => x.IsActive);                
                var jobScheduler = new HangfireJobScheduler();
                foreach (var job in jobs)
                {
                    jobScheduler.RemoveIfExists(job.Name + "~" + job.ScheduledJobId);
                    jobScheduler.AddOrUpdate(job);
                }
            }
            catch (System.Exception e)
            {                
                throw;
            }
        }
    }

    public class UnityHubActivator : IHubActivator
    {
        private readonly IUnityContainer container;

        public UnityHubActivator(IUnityContainer container)
        {
            this.container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)container.Resolve(descriptor.HubType);
        }
    }
}