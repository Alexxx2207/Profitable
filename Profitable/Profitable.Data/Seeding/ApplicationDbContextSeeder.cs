namespace Profitable.Data.Seeding
{
	using Profitable.Data.Seeding.Seeders;
	using Profitable.Data.Seeding.Seeders.Contracts;

	public class ApplicationDbContextSeeder : ISeeder
	{
		private readonly bool isProduction;
		public ApplicationDbContextSeeder(bool isProduction)
		{
			this.isProduction = isProduction;
		}

		public async Task SeedAsync(
			ApplicationDbContext dbContext,
			IServiceProvider serviceProvider)
		{
			if (dbContext == null)
			{
				throw new ArgumentNullException(nameof(dbContext));
			}

			if (serviceProvider == null)
			{
				throw new ArgumentNullException(nameof(serviceProvider));
			}

			var seeders = new List<ISeeder>()
			{
				new RoleSeeder(),
				new UsersSeeder(),
				new MarketTypesSeeder(),
				new ExchangesSeeder(),
				new FinantialInstrumentsSeeder(),
				new FuturesSeeder(),
				new COTReportsSeeder(),
				new BooksSeeder(),
			};

			if (!isProduction)
			{
				seeders.Add(new PostsSeeder());
			}


			foreach (var seeder in seeders)
			{
				await seeder.SeedAsync(dbContext, serviceProvider);
				await dbContext.SaveChangesAsync();
			}
		}
	}
}
