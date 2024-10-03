using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PraktikÖvning.Models;

namespace PraktikÖvning.Database
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Child>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

        }
        public DbSet<Child> Children { get; set; }
    }
}
