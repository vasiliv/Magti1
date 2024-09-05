using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Magti1.Models;

namespace Magti1.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }
        public DbSet<FreeNumber> FreeNumbers { get; set; }
        public DbSet<BoughtNumber> BoughtNumbers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // Seed initial data
            builder.Entity<FreeNumber>().HasData(
                new FreeNumber { Id = 1, PhoneNumber = 595604040 },
                new FreeNumber { Id = 2, PhoneNumber = 595580005 },
                new FreeNumber { Id = 3, PhoneNumber = 593146165 }
            );

            builder.Entity<ApplicationUser>()
            .HasMany(b => b.BoughtNumber)
            .WithOne(a => a.ApplicationUser)
            .HasForeignKey(a => a.ApplicationUserId);
        }
    }
}
