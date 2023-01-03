namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Models.RequestModels.Journals;
	using Profitable.Services.Journals.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;

	public class JournalsController : BaseApiController
	{
		private readonly IJournalService journalService;

		public JournalsController(IJournalService journalService)
		{
			this.journalService = journalService;
		}

		[HttpGet("get")]
		[Authorize]
		public async Task<IActionResult> Get(
			[FromQuery] int page,
			[FromQuery] int pageCount)
		{
			var result = await journalService.GetUserJournals(UserId, page, pageCount);

			return Ok(result);
		}

		[HttpGet("get/{journalId}")]
		[Authorize]
		public async Task<IActionResult> GetSpecific(
			[FromRoute] Guid journalId)
		{
			var result = await journalService.GetUserJournal(UserId, journalId);

			return Ok(result);
		}

		[HttpPost("add")]
		[Authorize]
		public async Task<IActionResult> Add(
			[FromBody] AddJournalRequestModel model)
		{
			var result = await journalService.AddUserJournals(UserId, model);

			if (result.Succeeded)
			{
				return Ok(result);
			}

			return BadRequest(result.Error);
		}

		[HttpPatch("update")]
		[Authorize]
		public async Task<IActionResult> Update(
			[FromBody] UpdateJournalRequestModel model)
		{
			var result = await journalService.UpdateUserJournals(UserId, model);

			if (result.Succeeded)
			{
				return Ok(result);
			}

			return BadRequest(result.Error);
		}

		[HttpDelete("delete")]
		[Authorize]
		public async Task<IActionResult> Update(
			[FromQuery] Guid journalId)
		{
			var result = await journalService.DeleteUserJournals(UserId, journalId);

			if (result.Succeeded)
			{
				return Ok(result);
			}

			return BadRequest(result.Error);
		}
	}
}
