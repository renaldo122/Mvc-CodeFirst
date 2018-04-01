using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Mvc.Domain.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Tables
        public DbSet<Funitor> Funitor { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Compare> Compare { get; set; }
        #endregion

        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
