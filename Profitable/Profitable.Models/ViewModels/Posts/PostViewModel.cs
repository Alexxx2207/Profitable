using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.ViewModels.Posts
{
    public class PostViewModel
    {
        public string GUID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public IEnumerable<LikeViewModel> Likes { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
