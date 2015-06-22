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
            app.UseHangfireDashboard();
            ConfigureAuth(app);
            app.MapSignalR();
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