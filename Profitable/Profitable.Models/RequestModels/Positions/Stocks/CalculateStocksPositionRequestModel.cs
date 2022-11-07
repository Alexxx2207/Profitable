namespace Profitable.Models.RequestModels.Positions.Stocks
{
    public class CalculateStocksPositionRequestModel
    {
        public double BuyPrice { get; set; }

        public double SellPrice { get; set; }

        public double NumberOfShares { get; set; }

        public double BuyCommission { get; set; }

        public double SellCommission { get; set; }

    }
}
