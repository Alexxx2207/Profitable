using Microsoft.AspNetCore.Identity;
using Profitable.Models.EntityModels;
using Profitable.Models.InputModels.Seedeing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                UserName = "as@as.as",
                FirstName = "Alexander",
                LastName = "Ivanov",
                Password = "123456",
                ProfilePicture = null
            }
        };
    }
}
