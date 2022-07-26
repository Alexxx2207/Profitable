using Profitable.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Profitable.Models.EntityModels
{
    public class MarketType : EntityBase
    {
        [Required]
        public string Name { get; set; }
    }
}
