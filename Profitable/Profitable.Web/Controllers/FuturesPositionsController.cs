using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profitable.Common.Enums;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Positions;
using Profitable.Models.RequestModels.Positions.Futures;
using Profitable.Models.ResponseModels.Positions;
using Profitable.Services.Positions.Contracts;
using Profitable.Services.Users.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;
using System.Globalization;
using System.Security.Claims;

namespace Profitable.Web.Controllers
{
    public class FuturesPositionsController : BaseApiController
    {
        private readonly IFuturesPositionsService futuresPositionsService;

        public FuturesPositionsController(
            IFuturesPositionsService futuresPositionsService)
        {
            this.futuresPositionsService = futuresPositionsService;
        }

        [HttpGet("records/{recordId}/positions")]
        public async Task<IActionResult> GetAllPositionsInARecord(
            [FromRoute] string recordId,
            [FromQuery] string afterDate,
            [FromQuery] string beforeDate)
        {
            if (afterDate != null && beforeDate != null)
            {
                var futuresPositions = await futuresPositionsService.GetFuturesPositions(
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
            try { 
                var positions = await futuresPositionsService.GetFuturesPositionByGuid(Guid.Parse(positionGuid));

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
            [FromBody] AddFuturesPositionRequestModel model)
        {
            var result =
                await futuresPositionsService.AddFuturesPositions(Guid.Parse(recordId), model, this.UserId);

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
           [FromBody] ChangeFuturesPositionRequestModel model)
        {

            var result = await futuresPositionsService.ChangeFuturesPosition(
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
            var result = await futuresPositionsService.DeleteFuturesPositions(
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
            [FromBody] CalculateFuturesPositionRequestModel model)
        {
            try
            {
                return Ok(futuresPositionsService.CalculateFuturesPosition(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
