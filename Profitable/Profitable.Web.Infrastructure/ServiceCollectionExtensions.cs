﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Profitable.Common.Automapper;
using Profitable.Data;
using Profitable.Models.EntityModels;
using Profitable.Services.Comments;
using Profitable.Services.Comments.Contracts;
using Profitable.Services.Common.Images;
using Profitable.Services.Common.Images.Contracts;
using Profitable.Services.Futures;
using Profitable.Services.Futures.Contracts;
using Profitable.Services.Markets;
using Profitable.Services.Markets.Contract;
using Profitable.Services.News;
using Profitable.Services.News.Contract;
using Profitable.Services.Positions;
using Profitable.Services.Positions.Contracts;
using Profitable.Services.Posts;
using Profitable.Services.Posts.Contracts;
using Profitable.Services.Search;
using Profitable.Services.Search.Contracts;
using Profitable.Services.Users;
using Profitable.Services.Users.Contracts;
using System.Text;

namespace Profitable.Web.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoles<IdentityRole<Guid>>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IMarketsService, MarketsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFuturesService, FuturesService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IPositionsRecordsService, PositionsRecordsService>();
            services.AddScoped<IUserSearchService, UserSearchService>();
            services.AddScoped<IPostSearchService, PostSearchService>();
            services.AddScoped<IPositionsService, PositionsService>();
            services.AddScoped<ICalculatorService, PositionsService>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            string JWT_KEY)
        {
            var key = Encoding.ASCII.GetBytes(JWT_KEY);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();

            return services;
        }

        public static IServiceCollection ConfigureAutomapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PostsMapper());
                mc.AddProfile(new LikesMapper());
                mc.AddProfile(new CommentsMapper());
                mc.AddProfile(new MarketsMapper());
                mc.AddProfile(new UsersMapper());
                mc.AddProfile(new FuturesContractsMapper());
                mc.AddProfile(new PositionsMapper());
                mc.AddProfile(new NewsMapper());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
