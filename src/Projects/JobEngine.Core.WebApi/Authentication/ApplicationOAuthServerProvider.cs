using JobEngine.Core.Persistence;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.WebApi
{
    public class ApplicationOAuthServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Required call
            await Task.FromResult(context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Here we can plug in any backing store to validate the credentials
            // Make sure you await the request to free up resources!

            if (context.UserName != "testUser" && context.Password != "password")
            {
                context.SetError(
                    "invalid_grant", "The user name or password is incorrect.");
                context.Rejected();
                return;
            }

            // Creates or retrieves a Claims Identity to represent the Authenticated user
            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("user_name", context.UserName));
            //identity.AddClaim(new Claim("expires_in", DateTime.Now.AddMinutes(Settings.RefreshTokenIntervalMinutes).ToString());

            // Add any new roles here
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            // Identitity will be encoded into a token in this call
            context.Validated(identity);
        }
    }
}

