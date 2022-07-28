using Microsoft.AspNetCore.Mvc;
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

        [Route("{username}")]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromRoute] string username)
        {
            var user = await userService.GetUserDetailsAsync(username);

            return Ok(user);
        }
    }
}
