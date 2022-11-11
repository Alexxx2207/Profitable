namespace Profitable.Web.Controllers.BaseApiControllers
{
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Common.Extensions;

	[Route("api/[controller]")]
	[ApiController]
	public class BaseApiController : ControllerBase
	{
		protected Guid UserId => User.Identity.GetUserId();

		protected string UserEmail => User.Identity.GetUserEmail();
	}
}
