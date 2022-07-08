using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.ViewModels.Comments
{
    public class CommentViewModel
    {
        public string GUID { get; set; }

        public Trader Author { get; set; }

        public int PostedOn { get; set; }

        public int Content { get; set; }
    }
}
