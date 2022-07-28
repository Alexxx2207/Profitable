using Profitable.Models.EntityModels.EntityBaseClass;

namespace Profitable.Models.EntityModels
{
    public class Tag : EntityBase
    {
        public string Name { get; set; }

        public ICollection<PostTag> OnPosts { get; set; }

    }
}
