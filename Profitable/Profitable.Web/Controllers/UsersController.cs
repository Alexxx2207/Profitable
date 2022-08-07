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
        public async Task<IActionResult> GetAsync()
        {
            var user = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

            var userInfo = await userService.GetUserDetailsAsync(user.Email);

            return Ok(userInfo);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel userRequestModel)
        {
            try
            {
                var token = await userService.LoginUserAsync(userRequestModel);

                return Ok(token);
            }
            catch (Exception error)
            {
                return Unauthorized(error.Message);
            }
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestModel userRequestModel)
        {
            try
            {
                var token = await userService.RegisterUserAsync(userRequestModel);

                return Ok(token);
            }
            catch (Exception error)
            {
                return Unauthorized(
                    string.Join(
                        Environment.NewLine,
                        error.Message
                            .Split(Environment.NewLine)
                            .Where(messages => messages != $"Username '{userRequestModel.Email}' is already taken."))
               );
            }
        }
    }
}
