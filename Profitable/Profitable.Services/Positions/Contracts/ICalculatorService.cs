using Profitable.Models.RequestModels.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Positions.Contracts
{
    public interface ICalculatorService
    {
        CalculateFuturesPositionResponseModel CalculateFuturesPosition(CalculateFuturesPositionRequestModel model);

        CalculateStocksPositionResponseModel CalculateStocksPosition(CalculateStocksPositionRequestModel model);

    }
}
