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

		public DbSet<ApplicationUser> Users { get; set; }

		public DbSet<Post> Posts { get; set; }

		public DbSet<Comment> Comments { get; set; }

		public DbSet<Like> Likes { get; set; }

		public DbSet<List> Lists { get; set; }

		public DbSet<ListsFinancialInstruments> ListsFinancialInstruments { get; set; }

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

			builder.Ignore<IdentityUserLogin<string>>();
			builder.Ignore<IdentityUserRole<string>>();
			builder.Ignore<IdentityUserClaim<string>>();
			builder.Ignore<IdentityUserToken<string>>();
			builder.Ignore<IdentityUser<string>>();

			builder.Entity<Comment>()
				.HasOne(c => c.Post)
				.WithMany(p => p.Comments)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Like>()
				.HasOne(c => c.Post)
				.WithMany(p => p.Likes)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}