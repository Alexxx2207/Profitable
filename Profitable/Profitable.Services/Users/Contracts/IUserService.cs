using Profitable.Models.ViewModels.Users;

namespace Profitable.Services.Users.Contracts
{
    public interface IUserService
    {
        Task<UserDetailsViewModel> GetUserDetails(string email);
    }
}
