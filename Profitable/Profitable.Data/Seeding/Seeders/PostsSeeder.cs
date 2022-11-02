using Profitable.Common.GlobalConstants;
using Profitable.Data.Repository;
using Profitable.Data.Seeding.Seeders.Contracts;
using Profitable.Models.EntityModels;
using System.Text.Json;

namespace Profitable.Data.Seeding.Seeders
{
    public class PostsSeeder : ISeeder
    {
        public async Task SeedAsync(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider = null)
        {
            var postsRepository = new Repository<Post>(dbContext);

            IAsyncEnumerable<JsonPost> postsInput = null;

            using (var stream = new FileStream(
                "DataToSeed/TestPosts.json",
                FileMode.Open,
                FileAccess.Read))
            {
                postsInput = JsonSerializer.DeserializeAsyncEnumerable<JsonPost>
                    (stream, new JsonSerializerOptions()
                    {
                        AllowTrailingCommas = true,
                        PropertyNameCaseInsensitive = true,
                    });

                var currentEntries = dbContext.Posts;

                var user = dbContext.Users
                    .First(u => u.Email == GlobalDatabaseConstants.DefaultUsersToSeed[0].Email);

                await foreach (var newPost in postsInput)
                {
                    if (!currentEntries.Any(p => p.Title == newPost.Title && p.Content == newPost.Content))
                    {
                        var postToAdd = new Post();

                        postToAdd.Title = newPost.Title;
                        postToAdd.Content = newPost.Content;
                        postToAdd.PostedOn = DateTime.UtcNow;
                        postToAdd.AuthorId = user.Id;
                        await postsRepository.AddAsync(postToAdd);
                    }
                }
            }
        }

        private class JsonPost
        {
            public string Title { get; set; }

            public string Content { get; set; }
        }
    }
}
