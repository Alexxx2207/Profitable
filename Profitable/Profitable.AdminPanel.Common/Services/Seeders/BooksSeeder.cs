namespace Profitable.AdminPanel.Common.Services.Seeders
{
	using Profitable.AdminPanel.Common.Services.Seeders.Contracts;
	using Profitable.Data;
	using Profitable.Models.EntityModels;
	using System.Text.Json;

	public class BooksSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext)
		{
			using (var stream = new FileStream(
                "Services/Seeders/DataToSeed/Books.json",
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
						var book = new Book();
						book.Title = instrument.Title;
						book.Authors = instrument.Authors;

						await dbContext.AddAsync(book);
					}
				}
			}
			await dbContext.SaveChangesAsync();
		}

		private class JsonInstrument
		{
			public string Title { get; set; }

			public string Authors { get; set; }
		}
	}
}
