using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.Owin;
using JobEngine.Core.Api;
using Hangfire;
using Hangfire.SqlServer;

namespace JobEngine.Core.WebApi
{
    // use an alias for the OWIN AppFunc:
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {            
            ConfigureAuth(appBuilder);

            appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnectionString");
            appBuilder.UseHangfireServer();
            //app.UseHangfireDashboard();

            //appBuilder.UseHangfire(config =>
            //{
            //    config.UseSqlServerStorage("HangfireConnectionString");
            //    config.UseServer();
            //});
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(3),
                RefreshTokenProvider = new ApplicationRefreshTokenProvider(),
                // Change before going to production
                AllowInsecureHttp = true
            };
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());        
        }
    }

}
