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
    }
}
