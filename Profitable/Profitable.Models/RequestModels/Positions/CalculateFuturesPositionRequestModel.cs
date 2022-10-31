namespace Profitable.Models.RequestModels.Positions
{
	public class CalculateFuturesPositionRequestModel
    {
		public string Direction { get; set; }

		public double EntryPrice { get; set; }

		public double ExitPrice { get; set; }

		public double Quantity { get; set; }

		public double TickSize { get; set; }

		public double TickValue { get; set; }
	}
}
