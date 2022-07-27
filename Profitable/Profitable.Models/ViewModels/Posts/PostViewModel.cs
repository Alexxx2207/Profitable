using Profitable.Models.ViewModels.Comments;
using Profitable.Models.ViewModels.Like;

namespace Profitable.Models.ViewModels.Posts
{
    public class PostViewModel
    {
        public string GUID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PostedOn { get; set; }

        public string Author { get; set; }

        public byte[] AuthorImage { get; set; }

        public string AuthorImageType { get; set; }

        public byte[] PostImage { get; set; }

        public string PostImageType { get; set; }

        public IEnumerable<LikeViewModel> Likes { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
