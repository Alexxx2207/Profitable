using Profitable.Models.EntityModels.EntityBaseClass;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profitable.Models.EntityModels
{
    public class Like : EntityBase
    {
        [ForeignKey("Trader")]
        public Guid TraderId { get; set; }
        public ApplicationUser Trader { get; set; }


        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
