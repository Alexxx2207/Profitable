namespace Profitable.Common.GlobalConstants
{
	public static class GlobalDatabaseConstants
	{
		public const int MESSAGE_MAX_LENGTH = 8000;

		public static readonly Dictionary<string, string> CotReportSourcesLinks = new Dictionary<string, string>
		{
			{
				"S&P 500 Consolidated - CHICAGO MERCANTILE EXCHANGE",
				"https://www.tradingster.com/cot/futures/fin/13874"
			},
			{
				"NASDAQ-100 Consolidated - CHICAGO MERCANTILE EXCHANGE",
				"https://www.tradingster.com/cot/futures/fin/20974"
			}
		};
	}
}
