using Profitable.Models.EntityModels.EntityBaseClass;
using System.ComponentModel.DataAnnotations;

namespace Profitable.Models.EntityModels
{
    public class FinancialInstrument : EntityBase
    {
        public FinancialInstrument()
        {
            ListsContainingIt = new HashSet<ListsFinancialInstruments>();

        }

        [Required]
        public string TickerSymbol { get; set; }

        [Required]
        public string ExchangeId { get; set; }
        public Exchange Exchange { get; set; }

        [Required]
        public string MarketTypeId { get; set; }

        public MarketType MarketType { get; set; }

        public ICollection<ListsFinancialInstruments> ListsContainingIt { get; set; }
    }
}
