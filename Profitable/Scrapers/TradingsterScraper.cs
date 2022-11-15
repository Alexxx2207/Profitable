using AngleSharp;

namespace Profitable.Common.Scraper
{
    public static class TradingsterScraper
    {
        public static async Task<List<ScrapeBigMoneyModel>> ScrapeBigMoneyPositions(string link, List<DateTime> dates)
        {
            var results = new List<ScrapeBigMoneyModel>();

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

        public class ScrapeBigMoneyModel
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
}