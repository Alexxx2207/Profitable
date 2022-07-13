using Profitable.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.EntityModels
{
    public class FinancialInstrument : EntityBase
    {
        public FinancialInstrument()
        {
            GUID = Guid.NewGuid().ToString();
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
