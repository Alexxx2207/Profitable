using Profitable.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.EntityModels
{
    public class Tag : EntityBase
    {
        public string Name { get; set; }

        public ICollection<PostTag> OnPosts { get; set; }

    }
}
