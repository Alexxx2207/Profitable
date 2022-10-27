using Microsoft.AspNetCore.Mvc;
using Profitable.Services.Markets.Contract;
using Profitable.Web.Controllers.BaseApiControllers;

namespace Profitable.Web.Controllers
{
    public class MarketsController : BaseApiController
    {
        private readonly IMarketsService marketsService;

        public MarketsController(IMarketsService marketsService)
        {
            this.marketsService = marketsService;
        }

        [HttpGet("instruments/{symbol}")]
        public async Task<IActionResult> GetInstrumentBySymbolAsync([FromRoute] string symbol)
        {
            var instrument =
                    await marketsService.GetFinantialInstrumentBySymbolAsync(symbol);

            return Ok(instrument);
        }

        [HttpGet("marketTypes/{marketType}/instruments")]
        public async Task<IActionResult> GetFinantialInstrumentsByMarketTypeAsync(
            [FromRoute] string marketType)
        {
            var instruments =
                    await marketsService.GetFinantialInstrumentsByTypeAsync(marketType);

            return Ok(instruments);
        }

        [HttpGet("instruments")]
        public async Task<IActionResult> GetAllInstrumentsAsync()
        {
            var instruments = await marketsService.GetAllFinantialInstrumentsAsync();

            return Ok(instruments);
        }

        [HttpGet("marketTypes")]
        public async Task<IActionResult> GetAllMarketTypesAsync()
        {
            var marketTypes = await marketsService.GetAllMarketTypesAsync();

            return Ok(marketTypes);
        }

        [HttpGet("instrument-groups")]
        public IActionResult GetAllInstrumentGroupsAsync()
        {
            var instrumentGroups = marketsService.GetAllInstrumentGroups();

            return Ok(instrumentGroups);
        }
    }
}
