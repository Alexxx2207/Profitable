using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.ViewModels.Posts
{
    public class CommentViewModel
    {
        public string GUID { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }
    }
}
