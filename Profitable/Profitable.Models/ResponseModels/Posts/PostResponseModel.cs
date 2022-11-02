using Profitable.Models.ResponseModels.Comments;
using Profitable.Models.ResponseModels.Like;

namespace Profitable.Models.ResponseModels.Posts
{
    public class PostResponseModel
    {
        public string Guid { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PostedOn { get; set; }

        public string AuthorEmail { get; set; }

        public string Author { get; set; }

        public byte[] AuthorImage { get; set; }

        public byte[] PostImage { get; set; }

        public string PostImageFileName { get; set; }

        public int CommentsCount { get; set; }

        public long LikesCount { get; set; }

        public bool IsLikedByTheUsed { get; set; }

        public IEnumerable<LikeResponseModel> Likes { get; set; }

        public IEnumerable<CommentResponseModel> Comments { get; set; }
    }
}
