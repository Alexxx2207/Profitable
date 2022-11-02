using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profitable.Common.Extensions;

namespace Profitable.Web.Controllers.BaseApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected Guid UserId => User.Identity.GetUserId();

        protected string UserEmail => User.Identity.GetUserEmail();
    }
}
