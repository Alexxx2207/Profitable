namespace Profitable.Data.Seeding.Seeders
{
	using AutoMapper;
	using Microsoft.EntityFrameworkCore;
	using Profitable.Common.GlobalConstants;
	using Profitable.Common.Scraper;
	using Profitable.Common.Services;
	using Profitable.Data.Seeding.Seeders.Contracts;
	using Profitable.Models.EntityModels;

	public class COTReportsSeeder : ISeeder
	{
		public async Task SeedAsync(
			ApplicationDbContext dbContext,
			IServiceProvider serviceProvider = null)
		{
			var lastTuesday = UtilityMethods.GetTheLast(DayOfWeek.Tuesday).AddDays(-7);

			var lastReport = await dbContext
				.COTReports
				.OrderByDescending(report => report.DatePublished)
				.FirstOrDefaultAsync();

			if (dbContext.COTReportedInstruments.Count() ==
					GlobalServicesConstants.CotReportSourcesLinks.Count &&
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

			var links = GlobalServicesConstants.CotReportSourcesLinks;
			var dates = UtilityMethods.GetPreviousDates(reportedWeeksToGet, lastTuesday);

			var mapper = (IMapper?)serviceProvider.GetService(typeof(IMapper));

			foreach (var link in links)
			{
				COTReportedInstrument? cotReportedInstrument = await dbContext.COTReportedInstruments
						.FirstOrDefaultAsync(instrument => instrument.InstrumentName == link.Key);

				if (cotReportedInstrument == null)
				{
					cotReportedInstrument = new COTReportedInstrument()
					{
						InstrumentName = link.Key,
					};

					dbContext.COTReportedInstruments.Add(cotReportedInstrument);
				}

				var reports = await TradingsterScraper.ScrapeBigMoneyPositions(link.Value, dates);

				foreach (var report in reports)
				{
					if (!dbContext.COTReports.Any(rep =>
						rep.DatePublished == report.DatePublished &&
						rep.COTReportedInstrumentId == cotReportedInstrument.Guid))
					{
						var cotReport = mapper.Map<COTReport>(
						   report,
						   opt => opt.AfterMap((src, dest) =>
						   {
							   dest.COTReportedInstrumentId = cotReportedInstrument.Guid;
						   }));

						dbContext.COTReports.Add(cotReport);
					}
				}
			}

			await dbContext.SaveChangesAsync();
		}
	}
}
