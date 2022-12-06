namespace Profitable.Data.Seeding.Seeders
{
	using Profitable.Data.Repository;
	using Profitable.Data.Seeding.Seeders.Contracts;
	using Profitable.Models.EntityModels;
	using System.Text.Json;

	public class BooksSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider = null)
		{
			var instrumentRepository = new Repository<Book>(dbContext);

			using (var stream = new FileStream(
				"DataToSeed/Books.json",
				FileMode.Open,
				FileAccess.Read))
			{
				var instrumentsInput = JsonSerializer.DeserializeAsyncEnumerable<JsonInstrument>
					(stream, new JsonSerializerOptions()
					{
						AllowTrailingCommas = true,
						PropertyNameCaseInsensitive = true,
					});

				var currentEntries = dbContext.Books;

				await foreach (var instrument in instrumentsInput)
				{
					if (!currentEntries.Any(e => e.Title == instrument.Title))
					{
						var finInstrument = new Book();
						finInstrument.Title = instrument.Title;
						finInstrument.Authors = instrument.Authors;

						await instrumentRepository.AddAsync(finInstrument);
					}
				}
			}
		}

		private class JsonInstrument
		{
			public string Title { get; set; }

			public string Authors { get; set; }
		}
	}
}
