using Profitable.Models.EntityModels;

namespace Profitable.Models.InputModels.Comments
{
    public class AddCommentInputModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public ApplicationUser Author { get; set; }

        public DateTime PostedOn { get; set; }
    }
}
