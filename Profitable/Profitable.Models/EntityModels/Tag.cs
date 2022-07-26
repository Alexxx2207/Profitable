using Profitable.Models.Contracts;

namespace Profitable.Models.EntityModels
{
    public class Tag : EntityBase
    {
        public string Name { get; set; }

        public ICollection<PostTag> OnPosts { get; set; }

    }
}
