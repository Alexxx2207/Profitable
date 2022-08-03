using Profitable.Common.Models;

namespace Profitable.Services.Users.Contracts
{
    public interface IJWTManagerRepository
    {
        JWTToken Authenticate(AuthUserModel users);
    }
}
