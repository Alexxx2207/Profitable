using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.ViewModels.Markets
{
    public class FinantialInstrumentViewModel
    {
        public string GUID { get; set; }

        public string Symbol { get; set; }

        public string ExchangeName { get; set; }

        public string MarketType { get; set; }
    }
}
