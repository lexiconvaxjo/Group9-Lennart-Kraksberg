using Microsoft.AspNet.Identity.EntityFramework;
using Project01.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project01.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext() : base("IdentityConnection")
        {

        }

        public static AppDbContext CreateConnection()
        {
            return new AppDbContext();
        }

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<People> Peoples { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Cart> Carts  { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

    }
}