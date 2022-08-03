using Profitable.Common.Models;
using Profitable.Models.RequestModels.Users;
using Profitable.Models.ResponseModels.Users;

namespace Profitable.Services.Users.Contracts
{
    public interface IUserService
    {
        Task<UserDetailsResponseModel> GetUserDetailsAsync(string email);

        Task<JWTToken> RegisterUser(RegisterUserRequestModel user);

        Task<JWTToken> LoginUser(LoginUserRequestModel user);
    }
}
