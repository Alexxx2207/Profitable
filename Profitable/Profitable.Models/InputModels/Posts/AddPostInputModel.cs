using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.InputModels.Posts
{
    public class AddPostInputModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Trader Author { get; set; }

        public DateTime PostedOn { get; set; }
    }
}
