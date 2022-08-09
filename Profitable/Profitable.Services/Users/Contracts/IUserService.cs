using Profitable.Common.Models;
using Profitable.Models.RequestModels.Users;
using Profitable.Models.ResponseModels.Users;

namespace Profitable.Services.Users.Contracts
{
    public interface IUserService
    {
        Task<UserDetailsResponseModel> GetUserDetailsAsync(string email);

        Task<UserDetailsResponseModel> EditUserAsync(string email, EditUserRequestModel EditUserData);

        Task<JWTToken> RegisterUserAsync(RegisterUserRequestModel user);

        Task<JWTToken> LoginUserAsync(LoginUserRequestModel user);
    }
}
