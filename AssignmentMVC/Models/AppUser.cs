using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Project01.Models
{
    public class AppUser : IdentityUser
    {      
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(
                user: this, authenticationType: DefaultAuthenticationTypes.ApplicationCookie
                );
            // Add custom user claims here
            return userIdentity;
        }


        public string City { get; set; }

        public string Country { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }



    }
}