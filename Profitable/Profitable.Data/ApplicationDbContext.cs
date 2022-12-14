namespace Profitable.Data
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Profitable.Models.EntityModels;

	public class ApplicationDbContext :
					IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public ApplicationDbContext()
		{ }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{ }

		public DbSet<Organization> Organizations { get; set; }

		public DbSet<FinancialInstrument> FinancialInstruments { get; set; }

		public DbSet<Exchange> Exchanges { get; set; }

		public DbSet<MarketType> MarketTypes { get; set; }

		public DbSet<FuturesContract> FuturesContracts { get; set; }

		public DbSet<PositionsRecordList> PositionsRecordLists { get; set; }

		public DbSet<TradePosition> TradePositions { get; set; }

		public DbSet<FuturesPosition> FuturesPositions { get; set; }

		public DbSet<StocksPosition> StocksPositions { get; set; }

		public DbSet<COTReportedInstrument> COTReportedInstruments { get; set; }

		public DbSet<COTReport> COTReports { get; set; }

		public DbSet<Book> Books { get; set; }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				var builder = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", optional: false);

				IConfiguration config = builder.Build();

				optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<IdentityUserClaim<Guid>>(entity =>
			{
				entity.ToTable(name: "UsersClaims");
			});

			builder.Entity<IdentityUserLogin<Guid>>(entity =>
			{
				entity.ToTable(name: "UsersLogins");
			});

			builder.Entity<IdentityUserToken<Guid>>(entity =>
			{
				entity.ToTable(name: "UsersTokens");
			});

			builder.Entity<IdentityUserRole<Guid>>(entity =>
			{
				entity.ToTable(name: "UsersRoles");
			});

			builder.Entity<IdentityRoleClaim<Guid>>(entity =>
			{
				entity.ToTable(name: "RolesClaims");
			});

			builder.Entity<IdentityRole<Guid>>(entity =>
			{
				entity.ToTable(name: "Roles");
			});

			builder.Entity<ApplicationUser>(entity =>
			{
				entity.ToTable(name: "Users");
			});
		}
	}
}