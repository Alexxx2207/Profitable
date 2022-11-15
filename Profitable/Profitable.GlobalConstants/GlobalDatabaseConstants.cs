namespace Profitable.Common.GlobalConstants
{
    using Profitable.Common.Models;
    using System.Collections.Generic;

    public static class GlobalDatabaseConstants
    {
        public static readonly string TraderRoleName = "Trader";

        public static readonly IReadOnlyList<SeededTrader> DefaultUsersToSeed = new List<SeededTrader>()
        {
            new SeededTrader()
            {
                Email = "as@as.as",
                FirstName = "Alexander",
                LastName = "Ivanov",
                Password = "123456",
                ProfilePictureURL = "",
            }
        };

        public static readonly int WeeksOfCOTReportsToSeed = 10;

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
