using Profitable.Models.EntityModels;

namespace Profitable.Models.RequestModels.Posts
{
    public class AddPostRequestModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
