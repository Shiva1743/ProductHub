using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductHub.Web.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>, IDataProtectionKeyContext
    {
        public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
    }
}
