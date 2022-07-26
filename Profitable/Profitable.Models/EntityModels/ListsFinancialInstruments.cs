using Profitable.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Profitable.Models.EntityModels
{
    public class ListsFinancialInstruments : EntityBase
    {
        [Required]
        public string ListId { get; set; }
        public List List { get; set; }

        [Required]
        public string FinancialInstrumentId { get; set; }
        public FinancialInstrument FinancialInstrument { get; set; }

    }
}
