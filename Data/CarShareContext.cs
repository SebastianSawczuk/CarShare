using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarShare.Models;
using CarShare.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarShare.Data
{
    public class CarShareContext : IdentityDbContext<CarShareUser>
    {
        public CarShareContext (DbContextOptions<CarShareContext> options)
            : base(options)
        {
        }
        public DbSet<CarShare.Models.Client> Client { get; set; } = default!;
        public DbSet<CarShare.Models.Car> Car { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<CarShare.Models.Adress> Adress { get; set; } = default!;
        public DbSet<CarShare.Models.Rent> Rent { get; set; } = default!;
    }
}
