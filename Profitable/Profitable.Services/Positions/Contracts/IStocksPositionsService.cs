using Profitable.Common.Models;
using Profitable.Models.RequestModels.Positions.Stocks;
using Profitable.Models.ResponseModels.Positions.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Positions.Contracts
{
    public interface IStocksPositionsService
    {
        Task<List<StocksPositionResponseModel>> GetStocksPositions(Guid recordId, DateTime afterDateFilter);

        Task<StocksPositionResponseModel> GetStocksPositionByGuid(Guid positionGuid);

        Task<Result> AddStocksPositions(Guid recordId, AddStocksPositionRequestModel model, Guid requesterGuid);

        Task<Result> ChangeStocksPosition(
            Guid recordId,
            Guid positionGuid,
            Guid requesterGuid,
            ChangeStocksPositionRequestModel model);

        Task<Result> DeleteStocksPositions(Guid recordId, Guid positionGuid, Guid requesterGuid);

        CalculateStocksPositionResponseModel CalculateStocksPosition(CalculateStocksPositionRequestModel model);
    }
}
