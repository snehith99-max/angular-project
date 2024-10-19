using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using StoryboardAPI.Controllers;



namespace StoryboardAPI.Authorization
{ 
    public class AuthorizationServerProvide :OAuthAuthorizationServerProvider
    {
        validateUser objvalidateuser = new validateUser(); 
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            LoginController objlogin = new LoginController();             
            
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var companyCode = string.Join(",",context.Scope);
            var formData = await context.Request.ReadFormAsync();
            var RouterPrefix = formData.Get("RouterPrefix") ?? "default";            
            var Username = formData.Get("username") ?? "default";                                   
            {
                var status = objvalidateuser.isvalid(Username, context.Password, RouterPrefix, companyCode);
                if (status == true)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                    identity.AddClaim(new Claim("username", "admin"));
                    identity.AddClaim(new Claim(ClaimTypes.Name, "Admin"));                    
                    context.Validated(identity);
                    objlogin.LoginErrorLog("success_login- '" + DateTime.Now);
                }
                else
                {
                    objlogin.LoginErrorLog("invalid_grant - Provided user credentials are incorrect- '" + DateTime.Now);
                    context.SetError("invalid_grant", "Provided user credentials are incorrect");
                    return;
                }
            }

        }
    }
}