using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sample.Domain.Entities.Identities;

namespace Sample.Identity
{
    public class SampleIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SampleIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
