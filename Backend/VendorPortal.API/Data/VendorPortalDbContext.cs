using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Models.Domain;

namespace VendorPortal.API.Data
{
    public class VendorPortalDbContext : DbContext
    {
        public VendorPortalDbContext(DbContextOptions<VendorPortalDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RFP> RFPs { get; set; }
        public DbSet<VendorCategory> VendorCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<VendorCategory>()
                .HasKey(e => new { e.VendorId,e.CategoryId});
        }


    }
}
