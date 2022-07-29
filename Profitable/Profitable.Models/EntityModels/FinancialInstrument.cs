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
        public Guid ExchangeId { get; set; }
        public Exchange Exchange { get; set; }

        [Required]
        public Guid MarketTypeId { get; set; }

        public MarketType MarketType { get; set; }

        public ICollection<ListsFinancialInstruments> ListsContainingIt { get; set; }
    }
}
