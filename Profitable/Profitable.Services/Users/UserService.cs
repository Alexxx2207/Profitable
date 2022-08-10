using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profitable.Common;
using Profitable.Common.Models;
using Profitable.Data.Repository.Contract;
using Profitable.GlobalConstants;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Users;
using Profitable.Models.ResponseModels.Users;
using Profitable.Services.Users.Contracts;
using System.Text.RegularExpressions;

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
                .Where(user => !user.IsDeleted)
                .FirstAsync(user => user.Email == email);

            return mapper.Map<UserDetailsResponseModel>(user);
        }

        public async Task<UserDetailsResponseModel> EditUserAsync(string email, EditUserRequestModel editUserData)
        {
            var user = await repository
                .GetAll()
                .Where(user => !user.IsDeleted)
                .FirstAsync(user => user.Email == email);

            if (user != null)
            {
                user.FirstName = editUserData.FirstName;
                user.LastName = editUserData.LastName;
                user.Description = editUserData.Description;

                repository.Update(user);
                await repository.SaveChangesAsync();

                return mapper.Map<UserDetailsResponseModel>(user);
            }
            else
            {
                throw new Exception("User was not edited");
            }
        }

        public async Task<JWTToken> LoginUserAsync(LoginUserRequestModel userRequestModel)
        {
            var user = await repository
                .GetAllAsNoTracking()
                .Where(user => !user.IsDeleted)
                .FirstAsync(user => user.Email == userRequestModel.Email);

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

        public async Task<UserDetailsResponseModel> EditUserPasswordAsync(ApplicationUser user, EditUserPasswordRequestModel editUserData)
        {
            if (!user.IsDeleted)
            {
                var result = await userManager.ChangePasswordAsync(user, editUserData.OldPassword, editUserData.newPassword);

                if (result.Succeeded)
                {
                    return mapper.Map<UserDetailsResponseModel>(user);
                }
                else
                {
                    throw new Exception("Invalid old password");
                }

            }
            else
            {
                throw new Exception("User was deleted");
            }
        }

        public async Task<UserDetailsResponseModel> EditUserProfileImageAsync(ApplicationUser user, EditUserProfileImageRequestModel editUserData)
        {
            if (!user.IsDeleted)
            {
                string time = Regex.Replace(DateTime.Today.ToString(), @"\/|\:|\s", "");
                string newFileName = time + editUserData.FileName;

                string path = GlobalServicesConstants.UploadsFolderPath +
                    GlobalServicesConstants.DirectorySeparatorChar +
                    ImageFor.Users.ToString() +
                    GlobalServicesConstants.DirectorySeparatorChar +
                    newFileName;

                await File.WriteAllBytesAsync(path, Convert.FromBase64String(editUserData.Image));

                if (user != null)
                {
                    user.ProfilePictureURL = newFileName;

                    repository.Update(user);
                    await repository.SaveChangesAsync();

                    return mapper.Map<UserDetailsResponseModel>(user);
                }
                else
                {
                    throw new Exception("User profile image was not edited");
                }
            }
            else
            {
                throw new Exception("User was deleted");
            }
        }

        public async Task<Result> SoftDeleteUserAsync(ApplicationUser user)
        {
            if (!user.IsDeleted)
            {
                repository.Delete(user);

                await repository.SaveChangesAsync();

                return true;
            }
            else
            {
                throw new Exception("User was deleted");
            }
        }

        public async Task<Result> HardDeleteUserAsync(ApplicationUser user)
        {
            if (!user.IsDeleted)
            {
                repository.HardDelete(user);

                await repository.SaveChangesAsync();

                return true;
            }
            else
            {
                throw new Exception("User was deleted");
            }
        }
    }
}
