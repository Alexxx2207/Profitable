namespace Profitable.Common.Scraper
{
    using AngleSharp;
    using Profitable.Common.Models.ScrapersModels;

    public static class TradingsterScraper
    {
        public static async Task<List<ScrapeBigMoneyPositionsModel>> ScrapeBigMoneyPositions(string link, List<DateTime> dates)
        {
            var results = new List<ScrapeBigMoneyPositionsModel>();

            var config = Configuration.Default.WithDefaultLoader();

            var context = BrowsingContext.New(config);

            for (int i = 0; i < dates.Count; i++)
            {
                var document = await context.OpenAsync($"{link}/{dates[i].Date.ToString("yyyy-MM-dd")}");

                var tableRowInstitutionsSelector = "div.chart-section table tbody tr:nth-child(2)";
                var tableRowLeveragedFundsSelector = "div.chart-section table tbody tr:nth-child(3)";

                var tableRowInstitutions = document.QuerySelector(tableRowInstitutionsSelector);
                var tableRowLeveragedFunds = document.QuerySelector(tableRowLeveragedFundsSelector);

                if (tableRowInstitutions != null && tableRowLeveragedFunds != null)
                {
                    var longInstitutionsInfo = tableRowInstitutions
                        .QuerySelector("td:nth-child(2)")?
                        .TextContent
                        .Replace(",", string.Empty)
                        .Split("\n");

                    var shortInstitutionsInfo = tableRowInstitutions
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

                    var longInstitutions = longInstitutionsInfo[0];
                    var longInstitutionsChange = longInstitutionsInfo[1];

                    var shortInstitutions = shortInstitutionsInfo[0];
                    var shortInstitutionsChange = shortInstitutionsInfo[1];

                    var longLeveragedFunds = longLeveragedFundsInfo[0];
                    var longLeveragedFundsChange = longLeveragedFundsInfo[1];

                    var shortLeveragedFunds = shortLeveragedFundsInfo[0];
                    var shortLeveragedFundsChange = shortLeveragedFundsInfo[1];

                    var modelInput = new ScrapeBigMoneyPositionsModel
                    {
                        AssetManagersLong = longInstitutions,
                        AssetManagersLongChange = longInstitutionsChange
                                                        .Replace("+", string.Empty),

                        AssetManagersShort = shortInstitutions,
                        AssetManagersShortChange = shortInstitutionsChange
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
    }
}