namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Models.EntityModels;
	using Profitable.Models.RequestModels.Users;
	using Profitable.Services.Users.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;
	using System.Security.Claims;

	public class UsersController : BaseApiController
	{
		private readonly IUserService userService;
		private readonly UserManager<ApplicationUser> userManager;

		public UsersController(
			IUserService userService,
			UserManager<ApplicationUser> userManager)
		{
			this.userService = userService;
			this.userManager = userManager;
		}

		[Authorize]
		[HttpGet("user")]
		public async Task<IActionResult> GetByJWTAsync()
		{
			try
			{
				var userEmail = this.User.FindFirstValue(ClaimTypes.Email);

				var userInfo = await userService.GetUserDetailsAsync(userEmail);

				return Ok(userInfo);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

		}

		[HttpGet("user/{email}")]
		public async Task<IActionResult> GetByEmailAsync(string email)
		{
			var userInfo = await userService.GetUserDetailsAsync(email);

			return Ok(userInfo);
		}

		[Authorize]
		[HttpGet("user/email")]
		public IActionResult GetEmailFromJWT()
		{
			return Ok(this.UserEmail);
		}

		[Authorize]
		[HttpGet("user/guid")]
		public async Task<IActionResult> GetGuidFromJWTAsync()
		{
			return Ok(this.UserId);
		}

		[Authorize]
		[HttpPatch("user/edit")]
		public async Task<IActionResult> EditGeneralDataAsync(
			[FromBody] EditUserRequestModel userRequestModel)
		{
			try
			{
				var userInfo = await userService.EditUserAsync(
					  this.UserId,
					  userRequestModel);

				return Ok(userInfo);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Authorize]
		[HttpPatch("user/edit/password")]
		public async Task<IActionResult> EditPasswordAsync(
			[FromBody] EditUserPasswordRequestModel userRequestModel)
		{
			try
			{
				var userResult = await userService.EditUserPasswordAsync(
					this.UserId,
					userRequestModel);

				return Ok();
			}
			catch (Exception error)
			{
				return BadRequest(error.Message);
			}
		}

		[Authorize]
		[HttpPatch("user/edit/profileImage")]
		public async Task<IActionResult> EditProfileImageAsync(
			[FromBody] EditUserProfileImageRequestModel userRequestModel)
		{
			var userResult =
					await userService.EditUserProfileImageAsync(
						this.UserId,
						userRequestModel);

			return Ok(userResult);
		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync(
			[FromBody] LoginUserRequestModel userRequestModel)
		{
			try
			{
				var token = await userService.LoginUserAsync(userRequestModel);

				return Ok(token);
			}
			catch (Exception error)
			{
				return BadRequest(error.Message);
			}
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync(
			[FromBody] RegisterUserRequestModel userRequestModel)
		{
			try
			{
				var token = await userService.RegisterUserAsync(userRequestModel);

				return Ok(token);
			}
			catch (Exception error)
			{
				return BadRequest(
					string.Join(
						Environment.NewLine,
						error.Message
							.Split(Environment.NewLine)
							.Where(messages =>
								messages != $"Username '{userRequestModel.Email}' is already taken."))
			   );
			}
		}

		[Authorize]
		[HttpDelete("{userEmail}/delete")]
		public async Task<IActionResult> DeleteAsync([FromRoute] string userEmail)
		{
			var result = await userService.HardDeleteUserAsync(this.UserId, userEmail);

			if (result.Succeeded)
			{
				return Ok(result.Succeeded);
			}
			else
			{
				return BadRequest(result.Error);
			}
		}

		[Authorize]
		[HttpDelete("{userEmail}/image/delete")]
		public async Task<IActionResult> DeleteUserImageAsync([FromRoute] string userEmail)
		{
			var result = await userService.DeleteUserImageAsync(this.UserId, userEmail);

			return Ok(result);
		}
	}
}
