using Profitable.Common;
using Profitable.Common.Models;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Users;
using Profitable.Models.ResponseModels.Users;

namespace Profitable.Services.Users.Contracts
{
    public interface IUserService
    {
        Task<UserDetailsResponseModel> GetUserDetailsAsync(string email);

        Task<Result> DeleteUserImageAsync(Guid requesterId, string emailOfDeleteUser);

        Task<Result> HardDeleteUserAsync(Guid requesterId, string emailOfDeleteUser);

        Task<UserDetailsResponseModel> EditUserAsync(Guid requesterId, EditUserRequestModel editUserData);

        Task<UserDetailsResponseModel> EditUserPasswordAsync(Guid requesterId, EditUserPasswordRequestModel editUserData);

        Task<UserDetailsResponseModel> EditUserProfileImageAsync(Guid requesterId, EditUserProfileImageRequestModel editUserData);

        Task<JWTToken> RegisterUserAsync(RegisterUserRequestModel user);

        Task<JWTToken> LoginUserAsync(LoginUserRequestModel user);
    }
}
