using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.RequestModels.Positions.Stocks
{
    public class AddStocksPositionRequestModel
    {
        public string Name { get; set; }

        public double EntryPrice { get; set; }

        public double ExitPrice { get; set; }

        public double QuantitySize { get; set; }

        public double BuyCommission { get; set; }

        public double SellCommission { get; set; }
    }
}
