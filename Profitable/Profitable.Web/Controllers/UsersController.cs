using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Users;
using Profitable.Services.Users.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;
using System.Security.Claims;

namespace Profitable.Web.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetByJWTAsync()
        {
            var userEmail = this.User.FindFirstValue(ClaimTypes.Email);

            var userInfo = await userService.GetUserDetailsAsync(userEmail);

            return Ok(userInfo);
        }

        [HttpGet("user/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var userInfo = await userService.GetUserDetailsAsync(email);

            return Ok(userInfo);
        }

        [Authorize]
        [HttpGet("user/email")]
        public async Task<IActionResult> GetEmailFromJWTAsync()
        {
            var userEmail = this.User.FindFirstValue(ClaimTypes.Email);

            return Ok(userEmail);
        }

        [Authorize]
        [HttpPatch("user/edit")]
        public async Task<IActionResult> EditGeneralDataAsync([FromBody] EditUserRequestModel userRequestModel)
        {
            var user = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

            if (user.Email == userRequestModel.Email)
            {
                var userInfo = await userService.EditUserAsync(user, userRequestModel);

                return Ok(userInfo);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpPatch("user/edit/password")]
        public async Task<IActionResult> EditPasswordAsync([FromBody] EditUserPasswordRequestModel userRequestModel)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

                var userResult = await userService.EditUserPasswordAsync(user, userRequestModel);

                return Ok(user);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [Authorize]
        [HttpPatch("user/edit/profileImage")]
        public async Task<IActionResult> EditProfileImageAsync([FromBody] EditUserProfileImageRequestModel userRequestModel)
        {
            var user = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

            var userResult = await userService.EditUserProfileImageAsync(user, userRequestModel);

            return Ok(userResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequestModel userRequestModel)
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
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequestModel userRequestModel)
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
                            .Where(messages => messages != $"Username '{userRequestModel.Email}' is already taken."))
               );
            }
        }

        [Authorize]
        [HttpDelete("user/delete")]
        public async Task<IActionResult> DeleteAsync()
        {
            var user = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

            var result = await userService.HardDeleteUserAsync(user);

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
        [HttpDelete("user/image/delete")]
        public async Task<IActionResult> DeleteUserImageAsync()
        {
            var user = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

            var result = await userService.DeleteUserImageAsync(user);

            return Ok(result);
        }
    }
}
