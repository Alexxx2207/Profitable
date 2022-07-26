using Profitable.Models.Contracts;

namespace Profitable.Models.EntityModels
{
    public class PostTag : EntityBase
    {
        public string PostId { get; set; }
        public Post Post { get; set; }

        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
