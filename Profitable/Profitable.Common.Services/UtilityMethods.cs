using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Common.Services
{
    public static class UtilityMethods
    {
        /// <summary>
        /// Get the date of the last <paramref name="dayOfWeek"/>.
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns>Previous day of the week date</returns>
        public static DateTime GetTheLast(DayOfWeek dayOfWeek)
        {
            var currentDate = DateTime.UtcNow;

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
