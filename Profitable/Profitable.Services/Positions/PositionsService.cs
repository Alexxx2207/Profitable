using Microsoft.EntityFrameworkCore;
using Profitable.Common.Enums;
using Profitable.Common.Models;
using Profitable.Common.Services;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Positions;
using Profitable.Models.ResponseModels.Positions;
using Profitable.Services.Positions.Contracts;

namespace Profitable.Services.Positions
{
	public class PositionsService : IPositionsService
	{
		private readonly IRepository<TradePosition> tradePositionsRepository;
		private readonly IRepository<FuturesPosition> futuresPositionsRepository;
		private readonly IRepository<FuturesPosition> stocksPositionsRepository;
		private readonly IRepository<FuturesContract> futuresContractRepository;

		public PositionsService(
			IRepository<TradePosition> tradePositionsRepository,
			IRepository<FuturesPosition> futuresPositionsRepository,
			IRepository<FuturesPosition> stocksPositionsRepository,
			IRepository<FuturesContract> futuresContractRepository)
		{
			this.tradePositionsRepository = tradePositionsRepository;
			this.futuresPositionsRepository = futuresPositionsRepository;
			this.stocksPositionsRepository = stocksPositionsRepository;
			this.futuresContractRepository = futuresContractRepository;
		}

		public async Task<List<PositionResponseModel>> GetFuturesPositions(Guid recordId, DateTime afterDateFilter)
		{
			var tradePositions = await tradePositionsRepository
				.GetAllAsNoTracking()
				.Include(p => p.PositionsRecordList)
				.Where(p => !p.IsDeleted)
				.Where(p => p.PositionAddedOn >= afterDateFilter)
				.Where(p => p.PositionsRecordListId == recordId)
				.ToListAsync();

			var results = new List<PositionResponseModel>();

			foreach (var position in tradePositions)
			{
				var futuresPosition = await futuresPositionsRepository
					.GetAllAsNoTracking()
					.Include(fp => fp.FuturesContract)
					.FirstAsync(p => p.TradePositionId == position.Guid);

				results.Add(new PositionResponseModel
				{
					PositionAddedOn = position.PositionAddedOn.ToString("F"),
					ContractName = futuresPosition.FuturesContract.Name,
					Direction = futuresPosition.Direction.ToString(),
					EntryPrice = position.EntryPrice.ToString(),
					ExitPrice = position.ExitPrice.ToString(),
					Quantity = position.QuantitySize.ToString(),
					TickSize = futuresPosition.FuturesContract.TickSize.ToString(),
					TickValue = futuresPosition.FuturesContract.TickValue.ToString(),
					PositionPAndL = position.RealizedProfitAndLoss.ToString()
				});
			}

			return results;
		}

		public async Task<Result> AddFuturesPositions(Guid recordId, AddFuturesPositionRequestModel model)
		{
			try
			{
				Enum.TryParse(model.Direction, out PositionDirection parsedPositionDirection);

				var position = new TradePosition
				{
					EntryPrice = model.EntryPrice,
					ExitPrice = model.ExitPrice,
					QuantitySize = model.Quantity,
					PositionsRecordListId = recordId,
					PositionAddedOn = DateTime.UtcNow,
					RealizedProfitAndLoss = CalculationFormulas.CalculateFuturesPL(
							parsedPositionDirection == PositionDirection.Long,
							model.EntryPrice,
							model.ExitPrice,
							model.Quantity,
							model.TickSize,
							model.TickValue),
				};

				var futuresContractGuid = (await futuresContractRepository
					.GetAllAsNoTracking()
					.FirstAsync(contract => contract.Name == model.ContractName)).Guid;

				var futuresPosition = new FuturesPosition
				{
					FuturesContractId = futuresContractGuid,
					TradePositionId = position.Guid
				};

				await tradePositionsRepository.AddAsync(position);

				await futuresPositionsRepository.AddAsync(futuresPosition);


				await tradePositionsRepository.SaveChangesAsync();

				await futuresPositionsRepository.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}
	}
}
