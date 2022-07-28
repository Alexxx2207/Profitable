using Profitable.Models.ResponseModels.Users;

namespace Profitable.Services.Users.Contracts
{
    public interface IUserService
    {
        Task<UserDetailsResponseModel> GetUserDetailsAsync(string email);
    }
}
