using Profitable.Models.EntityModels;

namespace Profitable.Models.RequestModels.Comments
{
    public class AddCommentRequestModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public ApplicationUser Author { get; set; }

        public DateTime PostedOn { get; set; }
    }
}
