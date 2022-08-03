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

        public async Task<JWTToken> LoginUser(LoginUserRequestModel userRequestModel)
        {
            var userExists = await userManager.FindByNameAsync(userRequestModel.Email);

            if (userExists != null &&
                await userManager.CheckPasswordAsync(userExists, userRequestModel.Password))
            {
                return jWTManagerRepository.Authenticate(new AuthUserModel
                {
                    Email = userRequestModel.Email,
                    Password = userRequestModel.Password,
                });
            }
            else
            {
                throw new Exception("User with this credentials doesn't exist");

            }

        }

        public async Task<JWTToken> RegisterUser(RegisterUserRequestModel userRequestModel)
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
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
                    Email = userRequestModel.Email,
                    Password = userRequestModel.Password,
                });
            }
        }
    }
}
