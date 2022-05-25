using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Sample.Persistence.DbContexts
{
    public class SampleDbContext : AuditDbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {

        }
    }
}
