using Profitable.Models.EntityModels.EntityBaseClass;
using System.ComponentModel.DataAnnotations;

namespace Profitable.Models.EntityModels
{
    public class ListsFinancialInstruments : EntityBase
    {
        [Required]
        public Guid ListId { get; set; }
        public List List { get; set; }

        [Required]
        public Guid FinancialInstrumentId { get; set; }
        public FinancialInstrument FinancialInstrument { get; set; }

    }
}
