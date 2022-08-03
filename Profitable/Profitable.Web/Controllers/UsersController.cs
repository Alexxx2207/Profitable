using Microsoft.AspNetCore.Mvc;
using Profitable.Models.RequestModels.Users;
using Profitable.Services.Users.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;

namespace Profitable.Web.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route("{email}")]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromRoute] string email)
        {
            var user = await userService.GetUserDetailsAsync(email);

            return Ok(user);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel userRequestModel)
        {
            try
            {
                var user = await userService.LoginUser(userRequestModel);

                return Ok(user);
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
                var user = await userService.RegisterUser(userRequestModel);

                return Ok(user);
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
