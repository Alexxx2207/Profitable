using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Profitable.Data;
using Profitable.Models.EntityModels;
using Profitable.Services.Comments;
using Profitable.Services.Comments.Contracts;
using Profitable.Services.Markets;
using Profitable.Services.Markets.Contract;
using Profitable.Services.Posts;
using Profitable.Services.Posts.Contracts;
using Profitable.Services.Users;
using Profitable.Services.Users.Contracts;

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

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IMarketsService, MarketsService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}