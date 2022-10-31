namespace Profitable.Common.Services
{
	public static class CalculationFormulas
	{
		public static double CalculateStocksPL(
			double buyPrice,
			double sellPrice,
			double numberOfShares,
			double buyCommission,
			double sellCommission)
		{
			return Math.Round(
                numberOfShares * sellPrice - sellCommission
				- 
				(numberOfShares * buyPrice + buyCommission)
				, 2);
		}
		
		public static double CalculateFuturesPL(
			bool directionBullish,
			double entryPrice,
			double exitPrice,
			double contractsCount,
			double tickSize,
			double tickValue)
		{
			int directionMultiplier = directionBullish ? 1 : -1;

			return Math.Round(
				(exitPrice - entryPrice) / tickSize
				* tickValue * contractsCount * directionMultiplier, 2);
		}
		
		public static double CalculateFuturesTicks(
			double entryPrice,
			double exitPrice,
			double contractsCount,
			double tickSize)
		{
			return Math.Round(
                (exitPrice - entryPrice) / tickSize * contractsCount, 2);
		}
	}
}
