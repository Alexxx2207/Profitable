using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Profitable.Models.EntityModels;
using Profitable.Data;

namespace Profitable.Web.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}