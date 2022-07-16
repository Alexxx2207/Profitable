using Newtonsoft.Json;
using Profitable.Data.Repository;
using Profitable.Data.Seeding.Seeders.Contracts;
using Profitable.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Data.Seeding.Seeders
{
    public class PostsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider = null)
        {
            var postsRepository = new Repository<Post>(dbContext);

            var json = JsonConvert.DeserializeObject<List<JsonPost>>(await new StreamReader("DataToSeed/TestPosts.json").ReadToEndAsync());

            var currentEntries = dbContext.Posts;

            var user = dbContext.Users
                .First(u => u.UserName == GlobalConstants.GlobalDatabaseConstants.DefaultUsersToSeed[0].UserName);

            foreach (var newPost in json)
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

        private class JsonPost
        {
            public string Title { get; set; }

            public string Content { get; set; }
        }
    }
}
