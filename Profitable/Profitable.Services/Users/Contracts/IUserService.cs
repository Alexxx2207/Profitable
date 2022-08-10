using Profitable.Common.Models;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Users;
using Profitable.Models.ResponseModels.Users;

namespace Profitable.Services.Users.Contracts
{
    public interface IUserService
    {
        Task<UserDetailsResponseModel> GetUserDetailsAsync(string email);

        Task<UserDetailsResponseModel> EditUserAsync(string email, EditUserRequestModel editUserData);

        Task<UserDetailsResponseModel> EditUserPasswordAsync(ApplicationUser user, EditUserPasswordRequestModel editUserData);

        Task<UserDetailsResponseModel> EditUserProfileImageAsync(ApplicationUser user, EditUserProfileImageRequestModel editUserData);

        Task<JWTToken> RegisterUserAsync(RegisterUserRequestModel user);

        Task<JWTToken> LoginUserAsync(LoginUserRequestModel user);
    }
}
