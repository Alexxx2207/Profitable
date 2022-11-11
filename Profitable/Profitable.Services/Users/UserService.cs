namespace Profitable.Services.Users
{
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Profitable.Common.Enums;
	using Profitable.Common.GlobalConstants;
	using Profitable.Common.Models;
	using Profitable.Data.Repository.Contract;
	using Profitable.Models.EntityModels;
	using Profitable.Models.RequestModels.Users;
	using Profitable.Models.ResponseModels.Users;
	using Profitable.Services.Common.Images.Contracts;
	using Profitable.Services.Users.Contracts;

	public class UserService : IUserService
	{
		private readonly IRepository<ApplicationUser> repository;
		private readonly IMapper mapper;
		private readonly IJWTManagerRepository jWTManagerRepository;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IImageService imageService;

		public UserService(
			IMapper mapper,
			IRepository<ApplicationUser> repository,
			IJWTManagerRepository jWTManagerRepository,
			UserManager<ApplicationUser> userManager,
			IImageService imageService)
		{
			this.repository = repository;
			this.mapper = mapper;
			this.jWTManagerRepository = jWTManagerRepository;
			this.userManager = userManager;
			this.imageService = imageService;
		}

		public async Task<UserDetailsResponseModel> GetUserDetailsAsync(string email)
		{
			var user = await repository
				.GetAllAsNoTracking()
				.Where(user => !user.IsDeleted)
				.FirstOrDefaultAsync(user => user.Email == email);

			if (user == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("User"));
			}

			return mapper.Map<UserDetailsResponseModel>(user);
		}

		public async Task<UserDetailsResponseModel> EditUserAsync(
			Guid requesterId,
			EditUserRequestModel editUserData)
		{
			var requester = await repository
				.GetAll()
				.Where(user => !user.IsDeleted)
				.FirstOrDefaultAsync(user => user.Id == requesterId);

			var userToEdit = await repository
			   .GetAll()
			   .Where(user => !user.IsDeleted)
			   .FirstOrDefaultAsync(user => user.Email == editUserData.Email);

			if (requester == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Requester"));
			}

			if (userToEdit == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("User To Edit"));
			}

			if (requester.Id != userToEdit.Id)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMesssage);
			}

			requester.FirstName = editUserData.FirstName;
			requester.LastName = editUserData.LastName;
			requester.Description = editUserData.Description;

			repository.Update(requester);

			await repository.SaveChangesAsync();

			return mapper.Map<UserDetailsResponseModel>(requester);
		}

		public async Task<JWTToken> LoginUserAsync(LoginUserRequestModel userRequestModel)
		{
			var user = await repository
				.GetAllAsNoTracking()
				.Where(user => !user.IsDeleted)
				.FirstOrDefaultAsync(user => user.Email == userRequestModel.Email);

			if (user != null &&
				await userManager.CheckPasswordAsync(user, userRequestModel.Password))
			{
				return jWTManagerRepository
						.Authenticate(mapper.Map<AuthUserModel>(user));
			}
			else
			{
				throw new Exception(
					"We have found you by email, but the provided password is invalid.");
			}
		}

		public async Task<JWTToken> RegisterUserAsync(RegisterUserRequestModel userRequestModel)
		{

			if (userRequestModel.FirstName.Length >= 2 && userRequestModel.LastName.Length >= 2)
			{
				var user = mapper.Map<ApplicationUser>(userRequestModel);

				var result = await userManager.CreateAsync(user, userRequestModel.Password);
				if (!result.Succeeded)
				{
					throw new Exception(
						string.Join(
							Environment.NewLine,
							result.Errors.Select(e => e.Description)));
				}
				else
				{
					return jWTManagerRepository
								.Authenticate(mapper.Map<AuthUserModel>(user));
				}
			}
			else
			{
				return null;
			}
		}

		public async Task<UserDetailsResponseModel> EditUserPasswordAsync(
			Guid requesterId,
			EditUserPasswordRequestModel editUserData)
		{
			var requester = await repository
				.GetAll()
				.Where(user => !user.IsDeleted)
				.FirstOrDefaultAsync(user => user.Id == requesterId);

			var userToEdit = await repository
			   .GetAll()
			   .Where(user => !user.IsDeleted)
			   .FirstOrDefaultAsync(user => user.Email == editUserData.Email);

			if (requester == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Requester"));
			}

			if (userToEdit == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("User To Edit"));
			}

			if (requester.Id != userToEdit.Id)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMesssage);
			}

			var result =
					 await userManager.ChangePasswordAsync(
						 requester, editUserData.OldPassword,
						 editUserData.NewPassword);

			if (result.Succeeded)
			{
				return mapper.Map<UserDetailsResponseModel>(requester);
			}
			else
			{
				throw new Exception("Invalid old password");
			}
		}

		public async Task<UserDetailsResponseModel> EditUserProfileImageAsync(
			Guid requesterId,
			EditUserProfileImageRequestModel editUserData)
		{

			var requester = await repository
				 .GetAll()
				 .Where(user => !user.IsDeleted)
				 .FirstOrDefaultAsync(user => user.Id == requesterId);

			var userToEdit = await repository
			   .GetAll()
			   .Where(user => !user.IsDeleted)
			   .FirstOrDefaultAsync(user => user.Email == editUserData.Email);

			if (requester == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Requester"));
			}

			if (userToEdit == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("User To Edit"));
			}

			if (requester.Id != userToEdit.Id)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMesssage);
			}

			if (!string.IsNullOrWhiteSpace(requester.ProfilePictureURL))
			{
				await imageService.DeleteUploadedImageAsync(
					ImageFor.Users,
					requester.ProfilePictureURL);
			}

			string newFileName =
					await imageService.SaveUploadedImageAsync(
						ImageFor.Users,
						editUserData.FileName,
						editUserData.Image);

			requester.ProfilePictureURL = newFileName;

			repository.Update(requester);

			await repository.SaveChangesAsync();

			return mapper.Map<UserDetailsResponseModel>(requester);
		}

		public async Task<Result> DeleteUserImageAsync(Guid requesterId, string emailOfDeleteUser)
		{
			var requester = await repository
				 .GetAll()
				 .Where(user => !user.IsDeleted)
				 .FirstOrDefaultAsync(user => user.Id == requesterId);

			var userToEdit = await repository
			   .GetAll()
			   .Where(user => !user.IsDeleted)
			   .FirstOrDefaultAsync(user => user.Email == emailOfDeleteUser);

			if (requester == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Requester"));
			}

			if (userToEdit == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("User To Edit"));
			}

			if (requester.Id != userToEdit.Id)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMesssage);
			}

			if (!string.IsNullOrWhiteSpace(requester.ProfilePictureURL))
			{
				await imageService.DeleteUploadedImageAsync(
					ImageFor.Users,
					requester.ProfilePictureURL);
			}

			requester.ProfilePictureURL = "";

			repository.Update(requester);

			await repository.SaveChangesAsync();

			return true;
		}

		public async Task<Result> HardDeleteUserAsync(Guid requesterId, string emailOfDeleteUser)
		{
			var requester = await repository
				 .GetAll()
				 .Where(user => !user.IsDeleted)
				 .FirstOrDefaultAsync(user => user.Id == requesterId);

			var userToEdit = await repository
			   .GetAll()
			   .Where(user => !user.IsDeleted)
			   .FirstOrDefaultAsync(user => user.Email == emailOfDeleteUser);

			if (requester == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Requester"));
			}

			if (userToEdit == null)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("User To Edit"));
			}

			if (requester.Id != userToEdit.Id)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMesssage);
			}

			if (!string.IsNullOrWhiteSpace(requester.ProfilePictureURL))
			{
				await imageService.DeleteUploadedImageAsync(
					ImageFor.Users,
					requester.ProfilePictureURL);
			}

			repository.HardDelete(requester);

			await repository.SaveChangesAsync();

			return true;
		}
	}
}
