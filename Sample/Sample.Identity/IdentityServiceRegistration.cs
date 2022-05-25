using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using Sample.Identity.Configuration;
using Sample.Identity.Services;
using Sample.Application.Contracts.Identity;
using Sample.Application.Models.Authentication;

namespace Sample.Identity
{
    public static class IdentityServiceRegistration
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("PgsqlSampleContext");
            var jwtSettings = new JwtSettings();
            configuration.Bind("JwtSettings", jwtSettings);
            SigningKeyConfiguration signinKeyConfiguration = new SigningKeyConfiguration(jwtSettings.SecretKey);
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddSingleton<ISigningKeyConfiguration>(signinKeyConfiguration);

            services.AddDbContext<SampleIdentityDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(SampleIdentityDbContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SampleIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("401 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("403 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                    };
                });

            services.AddAuthentication()
               .AddJwtBearer(AzureADDefaults.BearerAuthenticationScheme, x =>
               {
                   x.Authority = $"{configuration["AzureAd:Instance"]}{configuration["AzureAd:TenantId"]}/v2.0";
                   x.Audience = configuration["AzureAd:ClientId"];
               });
        }
    }
}
