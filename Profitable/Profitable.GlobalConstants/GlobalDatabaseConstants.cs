using Profitable.Common.Models;

namespace Profitable.GlobalConstants
{
    public class GlobalDatabaseConstants
    {
        public const string TraderRoleName = "Trader";

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
