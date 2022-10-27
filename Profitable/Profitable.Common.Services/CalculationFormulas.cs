namespace Profitable.Common.Services
{
	public static class CalculationFormulas
	{
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
	}
}
