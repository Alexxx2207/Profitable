namespace Profitable.Data.Seeding.Seeders
{
    using Profitable.Data.Seeding.Seeders.Contracts;
    using Profitable.Common.GlobalConstants;
    using System.ComponentModel.DataAnnotations;
    using AngleSharp;
    using Profitable.Models.EntityModels;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public class COTReportsSeeder : ISeeder
    {
        private List<DateTime> GetDates(int numberOfDates)
        {
            var results = new List<DateTime>();

            var currentDate = DateTime.UtcNow;

            while (currentDate.DayOfWeek != DayOfWeek.Tuesday)
                currentDate = currentDate.AddDays(-1);


            for (int i = 0; i < numberOfDates; i++)
            {
                currentDate = currentDate.AddDays(-7);
                results.Add(currentDate);
            }

            return results;
        }

        private async Task<List<ScrapeModel>> Scrape(string link, List<DateTime> dates)
        {
            var results = new List<ScrapeModel>();

            var config = Configuration.Default.WithDefaultLoader();

            var context = BrowsingContext.New(config);

            for (int i = 0; i < dates.Count; i++)
            {
                var document = await context.OpenAsync($"{link}/{dates[i].Date.ToString("yyyy-MM-dd")}");

                var tableRowIntitutionsSelector = "div.chart-section table tbody tr:nth-child(2)";
                var tableRowLeveragedFundsSelector = "div.chart-section table tbody tr:nth-child(3)";

                var tableRowIntitutions = document.QuerySelector(tableRowIntitutionsSelector);
                var tableRowLeveragedFunds = document.QuerySelector(tableRowLeveragedFundsSelector);

                if (tableRowIntitutions != null && tableRowLeveragedFunds != null)
                {
                    var longIntitutionsInfo = tableRowIntitutions
                        .QuerySelector("td:nth-child(2)")?
                        .TextContent
                        .Replace(",", string.Empty)
                        .Split("\n");                    
                    
                    var shortIntitutionsInfo = tableRowIntitutions
                        .QuerySelector("td:nth-child(5)")?
                        .TextContent
                        .Replace(",", string.Empty)
                        .Split("\n");
                    
                    var longLeveragedFundsInfo = tableRowLeveragedFunds
                        .QuerySelector("td:nth-child(2)")?
                        .TextContent
                        .Replace(",", string.Empty)
                        .Split("\n");                    
                    
                    var shortLeveragedFundsInfo = tableRowLeveragedFunds
                        .QuerySelector("td:nth-child(5)")?
                        .TextContent
                        .Replace(",", string.Empty)
                        .Split("\n");

                    var longIntitutions = longIntitutionsInfo[0];
                    var longIntitutionsChange = longIntitutionsInfo[1];

                    var shortIntitutions = shortIntitutionsInfo[0];
                    var shortIntitutionsChange = shortIntitutionsInfo[1];
                    
                    var longLeveragedFunds = longLeveragedFundsInfo[0];
                    var longLeveragedFundsChange = longLeveragedFundsInfo[1];

                    var shortLeveragedFunds = shortLeveragedFundsInfo[0];
                    var shortLeveragedFundsChange = shortLeveragedFundsInfo[1];

                    var modelInput = new ScrapeModel
                    {
                        AssetManagersLong = longIntitutions,
                        AssetManagersLongChange = longIntitutionsChange
                                                        .Replace("+", string.Empty),
                        
                        AssetManagersShort = shortIntitutions,
                        AssetManagersShortChange = shortIntitutionsChange
                                                        .Replace("+", string.Empty),
                        
                        LeveragedFundsLong = longLeveragedFunds,
                        LeveragedFundsLongChange = longLeveragedFundsChange
                                                        .Replace("+", string.Empty),
                        
                        LeveragedFundsShort = shortLeveragedFunds,
                        LeveragedFundsShortChange = shortLeveragedFundsChange
                                                        .Replace("+", string.Empty),

                        DatePublished = dates[i].Date,
                    };

                    results.Add(modelInput);
                }
            }

            return results;
        }

        public async Task SeedAsync(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider = null)
        {
            if(dbContext.COTReportedInstruments.Count() == 
                    GlobalDatabaseConstants.CotReportSourcesLinks.Count &&
                dbContext.COTReports.Count() >= 
                    GlobalDatabaseConstants.CotReportSourcesLinks.Count * GlobalDatabaseConstants.WeeksOfCOTReportsToSeed
                )
            {
                return;
            }

            var links = GlobalDatabaseConstants.CotReportSourcesLinks;
            var dates = GetDates(GlobalDatabaseConstants.WeeksOfCOTReportsToSeed);

            var mapper = (IMapper) serviceProvider.GetService(typeof(IMapper));

            foreach (var link in links)
            {
                COTReportedInstrument cotReportedInstrument = await dbContext.COTReportedInstruments
                        .FirstOrDefaultAsync(instrument => instrument.InstrumentName == link.Key); ;

                if (cotReportedInstrument == null)
                {
                    cotReportedInstrument = new COTReportedInstrument()
                    {
                        InstrumentName = link.Key,
                    };

                    dbContext.COTReportedInstruments.Add(cotReportedInstrument);
                }

                var reports = await Scrape(link.Value, dates);

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

    public class ScrapeModel
    {
        public DateTime DatePublished { get; set; }

        public string AssetManagersLong { get; set; }

        public string AssetManagersShort { get; set; }
        
        public string LeveragedFundsLong { get; set; }

        public string LeveragedFundsShort { get; set; }
        
        public string AssetManagersLongChange { get; set; }

        public string AssetManagersShortChange { get; set; }
        
        public string LeveragedFundsLongChange { get; set; }

        public string LeveragedFundsShortChange { get; set; }
    }
}
