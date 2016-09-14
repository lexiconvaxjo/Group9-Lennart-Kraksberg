using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Project01.Models;
using Project01.App_start;
using static Project01.App_start.AppUserManager;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity.Owin;

[assembly: OwinStartup(typeof(Project01.Startup))]

namespace Project01
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888



            app.CreatePerOwinContext(AppDbContext.CreateConnection);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.CreateUserManagerInstance);
            app.CreatePerOwinContext<AppSignIn>(AppSignIn.CreateSignInInstance);
            app.CreatePerOwinContext<AppRole>(AppRole.CreateRoleInstance);



            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                    SecurityStampValidator.OnValidateIdentity<AppUserManager, AppUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
        }
    }
}
