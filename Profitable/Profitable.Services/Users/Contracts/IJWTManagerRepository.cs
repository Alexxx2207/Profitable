namespace Profitable.Services.Users.Contracts
{
    using Profitable.Common.Models;
    public interface IJWTManagerRepository
    {
        JWTToken Authenticate(AuthUserModel users);
    }
}
