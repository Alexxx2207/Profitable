﻿namespace Profitable.Web.Infrastructure
{
	using AutoMapper;
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.IdentityModel.Tokens;
	using Profitable.Common.Automapper;
	using Profitable.Services.Books;
	using Profitable.Services.Books.Contracts;
	using Profitable.Services.Common.Images;
	using Profitable.Services.Common.Images.Contracts;
	using Profitable.Services.COT;
	using Profitable.Services.COT.Contracts;
	using Profitable.Services.Futures;
	using Profitable.Services.Futures.Contracts;
	using Profitable.Services.Journals;
	using Profitable.Services.Journals.Contracts;
	using Profitable.Services.Markets;
	using Profitable.Services.Markets.Contract;
	using Profitable.Services.News;
	using Profitable.Services.News.Contract;
	using Profitable.Services.Organizations;
	using Profitable.Services.Organizations.Contracts;
	using Profitable.Services.Positions;
	using Profitable.Services.Positions.Contracts;
	using Profitable.Services.Search;
	using Profitable.Services.Search.Contracts;
	using Profitable.Services.Users;
	using Profitable.Services.Users.Contracts;
	using System.Reflection;
	using System.Text;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services)
		{
			services.AddScoped<IImageService, ImageService>();
			services.AddScoped<IMarketsService, MarketsService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IFuturesService, FuturesService>();
			services.AddScoped<INewsService, NewsService>();
			services.AddScoped<IPositionsRecordsService, PositionsRecordsService>();
			services.AddScoped<IFuturesPositionsService, FuturesPositionsService>();
			services.AddScoped<IStocksPositionsService, StocksPositionsService>();
			services.AddScoped<ICOTService, COTService>();
			services.AddScoped<IBooksService, BooksService>();
			services.AddScoped<IBookSearchService, BookSearchService>();
			services.AddScoped<IUserSearchService, UserSearchService>();
			services.AddScoped<IOrganizationsService, OrganizationsService>();
			services.AddScoped<IOrganizationMembersService, OrganizationsService>();
			services.AddScoped<IJournalService, JournalService>();

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
			var profileTypes = Assembly.GetAssembly(typeof(AutoMapperConfiguration))
				.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(Profile)))
				.ToArray();

			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddMaps(profileTypes);
			});

			IMapper mapper = mapperConfig.CreateMapper();
			services.AddSingleton(mapper);

			return services;
		}
	}
}
