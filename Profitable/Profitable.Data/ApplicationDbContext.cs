﻿namespace Profitable.Data
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Profitable.Models.EntityModels;

	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext()
		{ }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{ }


		public DbSet<ApplicationUser> Users { get; set; }

		public DbSet<Journal> Journals { get; set; }

		public DbSet<Organization> Organizations { get; set; }

		public DbSet<OrganizationMessage> OrganizationsMessages { get; set; }

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
		}
	}
}