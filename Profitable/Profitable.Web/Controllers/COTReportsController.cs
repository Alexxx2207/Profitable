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

		[HttpGet("get/all-reports")]
		public async Task<IActionResult> GetAllReports()
		{
			try
			{
                var instruments = await cotService.GetReportedInstruments();

                return Ok(instruments);
            }
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		[HttpGet("get/report")]
		public async Task<IActionResult> GetReport([FromQuery] GetCOTRequestModel getCOTRequestModel)
		{
			try
			{
                var report = await cotService.GetReport(getCOTRequestModel);

                return Ok(report);
            }
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
