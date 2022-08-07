using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.Models;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Users;
using Profitable.Models.ResponseModels.Users;
using Profitable.Services.Users.Contracts;

namespace Profitable.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> repository;
        private readonly IMapper mapper;
        private readonly IJWTManagerRepository jWTManagerRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(
            IRepository<ApplicationUser> repository,
            IMapper mapper,
            IJWTManagerRepository jWTManagerRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.jWTManagerRepository = jWTManagerRepository;
            this.userManager = userManager;
        }

        public async Task<UserDetailsResponseModel> GetUserDetailsAsync(string email)
        {
            var user = await repository
                .GetAllAsNoTracking()
                .FirstAsync(user => user.Email == email);

            return mapper.Map<UserDetailsResponseModel>(user);
        }

        public async Task<JWTToken> LoginUserAsync(LoginUserRequestModel userRequestModel)
        {
            var user = await userManager.FindByEmailAsync(userRequestModel.Email);

            if (user != null &&
                await userManager.CheckPasswordAsync(user, userRequestModel.Password))
            {
                return jWTManagerRepository.Authenticate(new AuthUserModel
                {
                    Guid = user.Id.ToString(),
                    Email = user.Email,
                    UserName = user.UserName,
                });
            }
            else
            {
                throw new Exception("User with this credentials doesn't exist");
            }
        }

        public async Task<JWTToken> RegisterUserAsync(RegisterUserRequestModel userRequestModel)
        {

            if (userRequestModel.FirstName.Length >= 2 && userRequestModel.LastName.Length >= 2)
            {
                var user = new ApplicationUser()
                {
                    Email = userRequestModel.Email,
                    UserName = userRequestModel.Email,
                    FirstName = userRequestModel.FirstName,
                    LastName = userRequestModel.LastName,
                    ProfilePictureURL = userRequestModel.ProfilePictureURL,
                };

                var result = await userManager.CreateAsync(user, userRequestModel.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
                else
                {
                    return jWTManagerRepository.Authenticate(new AuthUserModel
                    {
                        Guid = user.Id.ToString(),
                        Email = user.Email,
                        UserName = user.UserName,
                    });
                }
            }
            else
            {
                return null;
            }
        }
    }
}
