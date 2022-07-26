using Profitable.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Profitable.Models.EntityModels
{

    public class Exchange : EntityBase
    {
        [Required]
        public string Name { get; set; }
    }
}
