namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Models.RequestModels.Positions.Stocks;
	using Profitable.Services.Positions.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;

	public class StocksPositionsController : BaseApiController
	{
		private readonly IStocksPositionsService stocksPositionsService;

		public StocksPositionsController(
			IStocksPositionsService stocksPositionsService)
		{
			this.stocksPositionsService = stocksPositionsService;
		}

		[HttpGet("records/{recordId}/positions")]
		public async Task<IActionResult> GetAllPositionsInARecord(
			[FromRoute] string recordId,
			[FromQuery] string afterDate,
			[FromQuery] string beforeDate)
		{
			if (afterDate != null && beforeDate != null)
			{
				var futuresPositions = await stocksPositionsService.GetStocksPositions(
						  Guid.Parse(recordId),
						  DateTime.Parse(afterDate),
						  DateTime.Parse(beforeDate));

				return Ok(futuresPositions);
			}

			return BadRequest();
		}

		[HttpGet("{positionGuid}")]
		public async Task<IActionResult> GetParticularPosition(
			[FromRoute] string positionGuid)
		{
			try
			{
				var positions = await stocksPositionsService.GetStocksPositionByGuid(Guid.Parse(positionGuid));

				return Ok(positions);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

		}

		[Authorize]
		[HttpPost("records/{recordId}/positions")]
		public async Task<IActionResult> CreatePosition(
			[FromRoute] string recordId,
			[FromBody] AddStocksPositionRequestModel model)
		{
			var result =
				await stocksPositionsService.AddStocksPositions(Guid.Parse(recordId), model, this.UserId);

			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return BadRequest(result.Error);
			}
		}

		[Authorize]
		[HttpPatch("records/{recordGuid}/positions/{positionGuid}/change")]
		public async Task<IActionResult> ChangePositionByGuid(
		   [FromRoute] string recordGuid,
		   [FromRoute] string positionGuid,
		[FromBody] ChangeStocksPositionRequestModel model)
		{

			var result = await stocksPositionsService.ChangeStocksPosition(
				Guid.Parse(recordGuid),
				Guid.Parse(positionGuid),
				this.UserId,
				model);

			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return BadRequest(result.Error);
			}
		}

		[Authorize]
		[HttpDelete("records/{recordId}/positions/{positionGuid}/delete")]
		public async Task<IActionResult> DeletePosition(
			[FromRoute] string recordId,
			[FromRoute] string positionGuid)
		{
			var result = await stocksPositionsService.DeleteStocksPositions(
				Guid.Parse(recordId),
				Guid.Parse(positionGuid),
				this.UserId);

			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return BadRequest(result.Error);
			}
		}

		[HttpPost("calculate-position")]
		public IActionResult CalculateFuturesPosition(
			[FromBody] CalculateStocksPositionRequestModel model)
		{
			try
			{
				return Ok(stocksPositionsService.CalculateStocksPosition(model));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
