namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Models.RequestModels.COT;
	using Profitable.Services.COT.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;

	public class COTReportsController : BaseApiController
	{
		private readonly ICOTService cotService;

		public COTReportsController(ICOTService cotService)
		{
			this.cotService = cotService;
		}

		[HttpGet("get")]
		public async Task<IActionResult> Get([FromQuery] GetCOTRequestModel getCOTRequestModel)
		{
			var report = await cotService.GetReport(getCOTRequestModel);

			return Ok(report);
		}
	}
}
