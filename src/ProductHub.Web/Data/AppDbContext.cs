using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductHub.Web.Models;

namespace ProductHub.Web.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>, IDataProtectionKeyContext
    {
        public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products {  get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>(e =>
            {
                e.Property(n => n.name).IsRequired().HasMaxLength(50);
                e.HasIndex(n => n.name).IsUnique();
            });

            builder.Entity<Product>(e => {
                e.Property(p => p.name).IsRequired().HasMaxLength(50);
                e.Property(p => p.description).HasMaxLength(100);
                e.Property(p => p.price).IsRequired().HasPrecision(5,2);
                e.Property(p => p.stock).IsRequired();
                e.HasIndex(p => p.name).IsUnique();
                e.Property(p => p.categoryId).IsRequired();

                e.HasOne(p => p.category)
                 .WithMany(p => p.Products)
                 .HasForeignKey(p => p.categoryId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
