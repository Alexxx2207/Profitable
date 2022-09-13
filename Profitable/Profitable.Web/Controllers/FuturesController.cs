using Microsoft.AspNetCore.Mvc;
using Profitable.Services.Futures.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;

namespace Profitable.Web.Controllers
{
	public class FuturesController : BaseApiController
	{
		private readonly IFuturesService futuresService;

		public FuturesController(IFuturesService futuresService)
		{
			this.futuresService = futuresService;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAllFututresContracts()
		{
			try
			{
				var futuresContracts = await futuresService.GetAllFuturesContracts();

				return Ok(futuresContracts);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
	}
}
