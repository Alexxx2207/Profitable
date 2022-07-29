using Profitable.Models.EntityModels.EntityBaseClass;

namespace Profitable.Models.EntityModels
{
    public class PostTag : EntityBase
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
