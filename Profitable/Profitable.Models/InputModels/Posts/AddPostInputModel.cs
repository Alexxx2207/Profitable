using Profitable.Models.EntityModels;

namespace Profitable.Models.InputModels.Posts
{
    public class AddPostInputModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
