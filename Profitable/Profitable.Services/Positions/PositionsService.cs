﻿using Microsoft.EntityFrameworkCore;
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
		private readonly IRepository<PositionsRecordList> positionsRecordListRepository;

		public PositionsService(
			IRepository<TradePosition> tradePositionsRepository,
			IRepository<FuturesPosition> futuresPositionsRepository,
			IRepository<FuturesPosition> stocksPositionsRepository,
			IRepository<FuturesContract> futuresContractRepository,
			IRepository<PositionsRecordList> positionsRecordListRepository)
		{
			this.tradePositionsRepository = tradePositionsRepository;
			this.futuresPositionsRepository = futuresPositionsRepository;
			this.stocksPositionsRepository = stocksPositionsRepository;
			this.futuresContractRepository = futuresContractRepository;
			this.positionsRecordListRepository = positionsRecordListRepository;
		}

		public async Task<List<PositionResponseModel>> GetFuturesPositions(Guid recordId, DateTime afterDateFilter)
		{
			

            var tradePositions = await tradePositionsRepository
				.GetAllAsNoTracking()
				.Include(p => p.PositionsRecordList)
				.Where(p => !p.IsDeleted)
				.Where(p => p.PositionAddedOn >= afterDateFilter)
				.Where(p => p.PositionsRecordListId == recordId)
				.OrderByDescending(p => p.PositionAddedOn)
				.ToListAsync();

			var results = new List<PositionResponseModel>();

			foreach (var position in tradePositions)
			{
				var futuresPosition = await futuresPositionsRepository
					.GetAllAsNoTracking()
					.Include(fp => fp.FuturesContract)
					.FirstAsync(p => p.TradePositionId == position.Guid);

				string parsedDirection = Enum.GetName(typeof(PositionDirection), futuresPosition.Direction);

                results.Add(new PositionResponseModel
				{
					Guid = position.Guid.ToString(),
					PositionAddedOn = position.PositionAddedOn.ToString("F"),
					ContractName = futuresPosition.FuturesContract.Name,
					Direction = parsedDirection,
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
				var dateTimeOfChange = DateTime.UtcNow;

                Enum.TryParse(model.Direction, out PositionDirection parsedPositionDirection);

                var positionsRecordUpdated = await positionsRecordListRepository
                    .GetAll()
                    .FirstAsync(record => record.Guid == recordId);

                positionsRecordUpdated.LastUpdated = dateTimeOfChange;

                var position = new TradePosition
				{
					EntryPrice = model.EntryPrice,
					ExitPrice = model.ExitPrice,
					QuantitySize = model.Quantity,
					PositionsRecordListId = recordId,
					PositionAddedOn = dateTimeOfChange,
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
					TradePositionId = position.Guid,
					Direction = parsedPositionDirection
                };

				await tradePositionsRepository.AddAsync(position);

				await futuresPositionsRepository.AddAsync(futuresPosition);


				await tradePositionsRepository.SaveChangesAsync();

				await futuresPositionsRepository.SaveChangesAsync();

				await positionsRecordListRepository.SaveChangesAsync();


                return true;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}
	}
}
