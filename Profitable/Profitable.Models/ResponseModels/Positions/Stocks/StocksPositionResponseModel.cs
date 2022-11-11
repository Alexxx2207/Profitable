namespace Profitable.Models.ResponseModels.Positions.Stocks
{
	public class StocksPositionResponseModel
	{
		public string Guid { get; set; }

		public string Name { get; set; }

		public DateTime PositionAddedOn { get; set; }

		public string Direction { get; set; }

		public string EntryPrice { get; set; }

		public string ExitPrice { get; set; }

		public string BuyCommission { get; set; }

		public string SellCommission { get; set; }

		public string QuantitySize { get; set; }

		public string RealizedProfitAndLoss { get; set; }
	}
}
