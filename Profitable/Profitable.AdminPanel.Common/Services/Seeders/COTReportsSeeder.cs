namespace Profitable.AdminPanel.Common.Services.Seeders
{
	using Microsoft.EntityFrameworkCore;
	using Profitable.AdminPanel.Common.Services.Seeders.Contracts;
	using Profitable.Common.GlobalConstants;
	using Profitable.Common.Scraper;
	using Profitable.Common.Services;
	using Profitable.Data;
	using Profitable.Models.EntityModels;

	public class COTReportsSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext)
		{
			var lastTuesday = UtilityMethods.GetTheLast(DayOfWeek.Tuesday).AddDays(-7);

			var lastReport = await dbContext.COTReports
				.OrderByDescending(report => report.DatePublished)
				.FirstOrDefaultAsync();

			var cotRepotedInstruments = dbContext.COTReportedInstruments;
			var links = GlobalDatabaseConstants.CotReportSourcesLinks;

			if (cotRepotedInstruments.Count() == links.Count &&
			   lastReport?.DatePublished.Date >= lastTuesday.Date)
			{
				return;
			}

			var reportedWeeksToGet = 0;

			if (lastReport == null)
			{
				reportedWeeksToGet = GlobalServicesConstants.WeeksOfCOTReportsForFirstSeed;
			}
			else
			{
				reportedWeeksToGet = (lastTuesday.Date - lastReport.DatePublished.Date).Days / 7 - 1;
			}

			var dates = UtilityMethods.GetPreviousDates(reportedWeeksToGet, lastTuesday);

			foreach (var link in links)
			{
				COTReportedInstrument? cotReportedInstrument = await cotRepotedInstruments
						.FirstOrDefaultAsync(instrument => instrument.InstrumentName == link.Key);

				if (cotReportedInstrument == null)
				{
					cotReportedInstrument = new COTReportedInstrument()
					{
						InstrumentName = link.Key,
					};

					await dbContext.AddAsync(cotReportedInstrument);
				}

				var reports = await TradingsterScraper.ScrapeBigMoneyPositions(link.Value, dates);

				foreach (var report in reports)
				{
					if (!dbContext.COTReports.Any(rep =>
						rep.DatePublished == report.DatePublished &&
						rep.COTReportedInstrumentId == cotReportedInstrument.Guid))
					{
						var cotReport = new COTReport()
						{
							AssetManagersLong = long.Parse(report.AssetManagersLong),
							AssetManagersShort = long.Parse(report.AssetManagersShort),
							AssetManagersLongChange = long.Parse(report.AssetManagersLongChange),
							AssetManagersShortChange = long.Parse(report.AssetManagersShortChange),
							LeveragedFundsLong = long.Parse(report.LeveragedFundsLong),
							LeveragedFundsShort = long.Parse(report.LeveragedFundsShort),
							LeveragedFundsLongChange = long.Parse(report.LeveragedFundsLongChange),
							LeveragedFundsShortChange = long.Parse(report.LeveragedFundsShortChange),
							COTReportedInstrumentId = cotReportedInstrument.Guid,
							DatePublished = report.DatePublished,
						};

						await dbContext.AddAsync(cotReport);
					};
				}
			}
			await dbContext.SaveChangesAsync();
		}
	}
}
