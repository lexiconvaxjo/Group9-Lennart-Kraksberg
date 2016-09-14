using Project01.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace Project01.App_start
{



    public class AppUserManager : UserManager<AppUser>
    {

        public AppUserManager(IUserStore<AppUser> store) : base(store)

        {
        }


        // App Roles
        public class AppRole : RoleManager<IdentityRole>

        {
            public AppRole(IRoleStore<IdentityRole, string> store) : base(store)
            {
            }


            public static AppRole CreateRoleInstance(IdentityFactoryOptions<AppRole> options, IOwinContext context)
            {
                var rost = new RoleStore<IdentityRole>(context.Get<AppDbContext>());
                return new AppRole(rost);
            }
        }





        public static AppUserManager CreateUserManagerInstance(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var um = new AppUserManager(new UserStore<AppUser>(context.Get<AppDbContext>()));

            um.UserValidator = new UserValidator<AppUser>(um)
            {

                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true

            };
            um.PasswordValidator = new PasswordValidator
            {

                RequireDigit = false,
                RequiredLength = 6,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false

            };

            //Forgotten password use encrypted
            var hashProvider = options.DataProtectionProvider;
            if (hashProvider != null)
            {
                 um.UserTokenProvider = new DataProtectorTokenProvider<AppUser>(hashProvider.Create("sdfljkleoiijfdkjaii"));

            }

            // Locked out values
            um.UserLockoutEnabledByDefault = true;
            um.MaxFailedAccessAttemptsBeforeLockout = 10;
            um.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(30);        
            return um;
        }





        // Katana and Owin
        public class AppSignIn : SignInManager<AppUser, string>

        {
            public AppSignIn(AppUserManager userManager, IAuthenticationManager authenticationManager)
                : base(userManager, authenticationManager)
            {
            }
            public static AppSignIn CreateSignInInstance(IdentityFactoryOptions<AppSignIn> options, IOwinContext context)
            {
                return new AppSignIn(context.GetUserManager<AppUserManager>(), context.Authentication);
            }

        }
    }



}