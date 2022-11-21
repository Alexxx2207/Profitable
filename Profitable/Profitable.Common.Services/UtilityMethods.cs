namespace Profitable.Common.Services
{
	public static class UtilityMethods
	{
		/// <summary>
		/// Get the last <paramref name="dayOfWeek"/> date.
		/// </summary>
		/// <param name="dayOfWeek"></param>
		/// <returns></returns>
		public static DateTime GetTheLast(DayOfWeek dayOfWeek)
		{
			var currentDate = DateTime.UtcNow;

			while (currentDate.DayOfWeek != dayOfWeek)
				currentDate = currentDate.AddDays(-1);

			return currentDate;
		}

		/// <summary>
		/// Get the last <paramref name="dayOfWeek"/> from <paramref name="fromDate"/>.
		/// </summary>
		/// <param name="dayOfWeek"></param>
		/// <returns></returns>
		public static DateTime GetTheLastDayOfWeekFromDate(DayOfWeek dayOfWeek, DateTime fromDate)
		{
			var currentDate = fromDate;

			while (currentDate.DayOfWeek != dayOfWeek)
				currentDate = currentDate.AddDays(-1);

			return currentDate;
		}

		/// <summary>
		/// Get the previous dates of the given day of week of the given <paramref name="fromTuesday"/>.
		/// fromTuesday is included always.
		/// </summary>
		/// <param name="numberOfDates"></param>
		/// <param name="fromTuesday"></param>
		/// <returns>Prior day of the week dates</returns>
		public static List<DateTime> GetPreviousDates(int numberOfDates, DateTime fromTuesday)
		{
			var results = new List<DateTime>();

			var currentDate = fromTuesday;
			results.Add(currentDate);


			for (int i = 0; i < numberOfDates; i++)
			{
				currentDate = currentDate.AddDays(-7);
				results.Add(currentDate);
			}

			return results;
		}
	}
}
