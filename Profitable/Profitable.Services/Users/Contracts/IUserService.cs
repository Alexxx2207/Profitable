using Profitable.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Users.Contracts
{
    public interface IUserService
    {
        Task<UserDetailsViewModel> GetUserDetails(string email);
    }
}
