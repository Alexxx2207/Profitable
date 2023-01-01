namespace Profitable.Services.Users
{
	using AutoMapper;
	using Microsoft.EntityFrameworkCore;
	using Profitable.Common.Enums;
	using Profitable.Common.GlobalConstants;
	using Profitable.Common.Models;
	using Profitable.Common.Services;
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
		private readonly IImageService imageService;

		public UserService(
			IMapper mapper,
			IRepository<ApplicationUser> repository,
			IJWTManagerRepository jWTManagerRepository,
			IImageService imageService)
		{
			this.repository = repository;
			this.mapper = mapper;
			this.jWTManagerRepository = jWTManagerRepository;
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
				.FirstOrDefaultAsync(user => user.Guid == requesterId);

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

			if (requester.Guid != userToEdit.Guid)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMessage);
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

			if (user == null)
			{
				throw new ArgumentException(
					GlobalServicesConstants.EntityDoesNotExist("User"));
			}
			else if (!Security.VerifyPassword(
				userRequestModel.Password,
				user.Guid,
				user.Salt,
				user.PasswordHash))
			{
				throw new ArgumentException(
					"We have found you by email, but the provided password is invalid.");

			}

			return jWTManagerRepository
						.Authenticate(mapper.Map<AuthUserModel>(user));
		}

		public async Task<JWTToken> RegisterUserAsync(RegisterUserRequestModel userRequestModel)
		{
			if (repository.GetAllAsNoTracking().Any(u => u.Email == userRequestModel.Email))
			{
				throw new InvalidOperationException(
					"Internal Server Error");
			}

			try
			{
				if (userRequestModel.FirstName.Length >= 2 &&
					userRequestModel.LastName.Length >= 2 &&
					userRequestModel.Password.Length >= GlobalServicesConstants.PasswordMinLength &&
					!string.IsNullOrWhiteSpace(userRequestModel.Email) &&
					!string.IsNullOrWhiteSpace(userRequestModel.Password))
				{
					var user = mapper.Map<ApplicationUser>(userRequestModel);

					var hashPasswordResult = Security.HashUserPassword(userRequestModel.Password, user.Guid);

					user.PasswordHash = hashPasswordResult.PasswordHash;
					user.Salt = hashPasswordResult.Salt;

					await repository.AddAsync(user);

					await repository.SaveChangesAsync();

					return jWTManagerRepository
									.Authenticate(mapper.Map<AuthUserModel>(user));
				}
				else
				{
					return null;
				}
			}
			catch (Exception)
			{
				throw new Exception(
						"Internal Server Error");
			}
		}

		public async Task<UserDetailsResponseModel> EditUserPasswordAsync(
			Guid requesterId,
			EditUserPasswordRequestModel editUserData)
		{
			var requester = await repository
				.GetAll()
				.Where(user => !user.IsDeleted)
				.FirstOrDefaultAsync(user => user.Guid == requesterId);

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

			if (requester.Guid != userToEdit.Guid)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMessage);
			}

			var result = await ChangePasswordAsync(
						 userToEdit,
						 editUserData.NewPassword);

			if (result)
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
				 .FirstOrDefaultAsync(user => user.Guid == requesterId);

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

			if (requester.Guid != userToEdit.Guid)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMessage);
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
				 .FirstOrDefaultAsync(user => user.Guid == requesterId);

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

			if (requester.Guid != userToEdit.Guid)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMessage);
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
				 .FirstOrDefaultAsync(user => user.Guid == requesterId);

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

			if (requester.Guid != userToEdit.Guid)
			{
				throw new Exception(GlobalServicesConstants.RequesterNotOwnerMessage);
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

		public async Task<string> GetUserOrganization(Guid userToCheckId, string requesterEmail)
		{
			var user = await repository
				.GetAllAsNoTracking()
				.FirstOrDefaultAsync(x => x.Guid == userToCheckId);

			if (user == null)
			{
				throw new InvalidOperationException(GlobalServicesConstants.EntityDoesNotExist("User to check"));
			}

			if (user.Email != requesterEmail)
			{
				throw new UnauthorizedAccessException("Requester not authorized to check!");
			}

			return user.OrganizationId?.ToString() ?? "";
		}

		private async Task<bool> ChangePasswordAsync(ApplicationUser user, string newPassword)
		{
			return await Task.Run(() =>
			{
				try
				{
					var hashResult = Security.HashUserPassword(newPassword, user.Guid);
					user.PasswordHash = hashResult.PasswordHash;
					user.Salt = hashResult.Salt;

					return true;
				}
				catch (Exception)
				{
					return false;
				}
			});
		}
	}
}
