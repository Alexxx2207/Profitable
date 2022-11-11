namespace Profitable.Models.RequestModels.Positions.Stocks
{
	public class ChangeStocksPositionRequestModel
	{
		public string Name { get; set; }

		public double EntryPrice { get; set; }

		public double ExitPrice { get; set; }

		public double QuantitySize { get; set; }

		public double BuyCommission { get; set; }

		public double SellCommission { get; set; }
	}
}
