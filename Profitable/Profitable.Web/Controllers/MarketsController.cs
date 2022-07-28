using Microsoft.AspNetCore.Mvc;
using Profitable.Services.Markets.Contract;
using Profitable.Web.Controllers.Contracts;

namespace Profitable.Web.Controllers
{
    public class MarketsController : BaseApiController
    {
        private readonly IMarketsService marketsService;

        public MarketsController(IMarketsService marketsService)
        {
            this.marketsService = marketsService;
        }

        [Route("instruments/{symbol}")]
        [HttpGet]
        public async Task<IActionResult> GetInstrumentBySymbolAsync([FromRoute] string symbol)
        {
            var instrument = await this.marketsService.GetFinantialInstrumentBySymbolAsync(symbol);

            return Ok(instrument);
        }

        [Route("marketTypes/{marketType}/instruments")]
        [HttpGet]
        public async Task<IActionResult> GetFinantialInstrumentsByMarketTypeAsync([FromRoute] string marketType)
        {
            var instruments = await this.marketsService.GetFinantialInstrumentsByTypeAsync(marketType);

            return Ok(instruments);
        }

        [Route("instruments")]
        [HttpGet]
        public async Task<IActionResult> GetAllInstrumentsAsync()
        {
            var instruments = await this.marketsService.GetAllFinantialInstrumentsAsync();

            return Ok(instruments);
        }

        [Route("marketTypes")]
        [HttpGet]
        public async Task<IActionResult> GetAllMarketTypesAsync()
        {
            var marketTypes = await this.marketsService.GetAllMarketTypesAsync();

            return Ok(marketTypes);
        }
    }
}
