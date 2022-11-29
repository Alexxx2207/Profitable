namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Common.Enums;
	using Profitable.Models.RequestModels.Positions.Records;
	using Profitable.Models.ResponseModels.Positions.Records;
	using Profitable.Services.Positions.Contracts;
	using Profitable.Services.Users.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;

	public class PositionRecordsController : BaseApiController
	{
		private readonly IPositionsRecordsService positionsRecordsService;
		private readonly IUserService userService;

		public PositionRecordsController(
			IPositionsRecordsService positionsRecordsService,
			IUserService userService)
		{
			this.positionsRecordsService = positionsRecordsService;
			this.userService = userService;
		}

		[HttpPost("by-user")]
		public async Task<IActionResult> GetAllPositionsRecordsByUser(
			[FromBody] GetUserPositionsRecordsRequestModel query)
		{
			try
			{
				var userGuid = Guid.Parse((await userService.GetUserDetailsAsync(query.UserEmail)).Guid);

				Enum.TryParse(query.OrderPositionsRecordBy, out OrderPositionsRecordBy orderBy);

				var records = await positionsRecordsService.GetUserRecordsAsync(
					userGuid,
					query.Page,
					query.PageCount,
					orderBy);


				return Ok(records);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

		}

		[HttpPost("by-user/by-instrument-group")]
		public async Task<IActionResult> GetAllPositionsRecordsByUser(
			[FromBody] GetUserPositionsRecordsByInstrumentGroupRequestModel query)
		{
			try
			{
				var userGuid = Guid.Parse((await userService.GetUserDetailsAsync(query.UserEmail)).Guid);

				Enum.TryParse(query.InstrumentGroup, out InstrumentGroup instrumentGroup);

				var records = await positionsRecordsService.GetUserRecordsByInstrumentGroupAsync(
					userGuid,
					query.Page,
					query.PageCount,
					instrumentGroup);


				return Ok(records);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

		}

		[HttpGet("order-options")]
		public IActionResult GetAllPositionsRecordsOrderByTypes()
		{
			return Ok(new OrderPositionsRecordsByTypes
			{
				Types = positionsRecordsService.GetPositionsRecordsOrderTypes()
			});
		}

		[Authorize]
		[HttpPost("create")]
		public async Task<IActionResult> CreatePositionsRecord(
			[FromBody] AddPositionsRecordRequestModel model)
		{
			try
			{
				var result = await positionsRecordsService.AddPositionsRecordList(
						this.UserId,
						model.RecordName,
						model.InstrumentGroup);

				if (result.Succeeded)
				{
					return Ok();
				}
				else
				{
					return BadRequest(result.Error);
				}
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Authorize]
		[HttpPatch("change/{recordGuid}")]
		public async Task<IActionResult> ChangePositionsRecord(
			[FromRoute] string recordGuid,
			[FromBody] ChangePositionsRecordRequestModel model)
		{

			var result = await positionsRecordsService.ChangeNamePositionsRecordList(
				Guid.Parse(recordGuid),
				model.RecordName,
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

		[Authorize]
		[HttpDelete("delete/{recordGuid}")]
		public async Task<IActionResult> DeletePositionsRecord([FromRoute] string recordGuid)
		{

			var result = await positionsRecordsService.DeletePositionsRecordList(
				Guid.Parse(recordGuid),
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
	}
}
