using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContexts.SampleDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PgsqlSampleContext"), sqloption =>
                {
                    sqloption.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                });
            });
            return services;
        }
    }
}
