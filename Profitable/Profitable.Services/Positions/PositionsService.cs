using AutoMapper;
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
	public class PositionsService : IPositionsService, ICalculatorService
    {
		private readonly IRepository<TradePosition> tradePositionsRepository;
		private readonly IRepository<FuturesPosition> futuresPositionsRepository;
		private readonly IRepository<FuturesPosition> stocksPositionsRepository;
		private readonly IRepository<FuturesContract> futuresContractRepository;
		private readonly IRepository<PositionsRecordList> positionsRecordListRepository;
		private readonly IMapper mapper;

		public PositionsService(
			IRepository<TradePosition> tradePositionsRepository,
			IRepository<FuturesPosition> futuresPositionsRepository,
			IRepository<FuturesPosition> stocksPositionsRepository,
			IRepository<FuturesContract> futuresContractRepository,
			IRepository<PositionsRecordList> positionsRecordListRepository,
			IMapper mapper)
		{
			this.tradePositionsRepository = tradePositionsRepository;
			this.futuresPositionsRepository = futuresPositionsRepository;
			this.stocksPositionsRepository = stocksPositionsRepository;
			this.futuresContractRepository = futuresContractRepository;
			this.positionsRecordListRepository = positionsRecordListRepository;
			this.mapper = mapper;
		}

		public async Task<List<PositionResponseModel>> GetFuturesPositions(
			Guid recordId,
			DateTime afterDateFilter)
		{
			var tradePositions = await tradePositionsRepository
				.GetAllAsNoTracking()
				.Include(p => p.PositionsRecordList)
				.Where(p =>
					!p.IsDeleted &&
					p.PositionAddedOn >= afterDateFilter &&
					p.PositionsRecordListId == recordId)
				.OrderByDescending(p => p.PositionAddedOn)
				.ToListAsync();

			var results = new List<PositionResponseModel>();

			foreach (var position in tradePositions)
			{
				var futuresPosition = await futuresPositionsRepository
					.GetAllAsNoTracking()
                    .Where(position => !position.IsDeleted)
                    .Include(fp => fp.FuturesContract)
					.FirstOrDefaultAsync(p => p.TradePositionId == position.Guid);

				if(futuresPosition == null)
				{
					continue;
				}

				string parsedDirection = Enum.GetName(typeof(PositionDirection), futuresPosition.Direction);

				var responseModel = mapper.Map(
					futuresPosition.FuturesContract, 
					mapper.Map<PositionResponseModel>(
						position,
						opt => opt.AfterMap((src, dest) =>
						{
							dest.Direction = parsedDirection;
						})
					)
				);

                results.Add(responseModel);
			}

			return results;
		}

		public async Task<PositionResponseModel> GetFuturesPositionByGuid(Guid positionGuid)
		{
			var tradePosition = await tradePositionsRepository
				.GetAllAsNoTracking()
				.Where(position => !position.IsDeleted)
				.FirstOrDefaultAsync(position => position.Guid == positionGuid);
			
			var futuresPosition = await futuresPositionsRepository
				.GetAllAsNoTracking()
				.Where(position => !position.IsDeleted)
				.Include(position => position.FuturesContract)
				.FirstOrDefaultAsync(position => position.TradePositionId == positionGuid);

			if(tradePosition == null)
			{
				throw new Exception("Trade position not found");
			}
			else if(futuresPosition == null)
			{
                throw new Exception("Futures position not found");
            }

			string parsedDirection = Enum.GetName(typeof(PositionDirection), futuresPosition.Direction);

            var responseModel = mapper.Map(
                    futuresPosition.FuturesContract,
                    mapper.Map<PositionResponseModel>(
                        tradePosition,
                        opt => opt.AfterMap((src, dest) =>
                        {
                            dest.Direction = parsedDirection;
                        })
                    )
                );

            return responseModel;
		}

		public async Task<Result> AddFuturesPositions(Guid recordId, AddFuturesPositionRequestModel model)
		{
			var dateTimeOfChange = DateTime.UtcNow;

			var isConvertedSuccessfully = 
							Enum.TryParse(model.Direction, out PositionDirection parsedPositionDirection);

			if(!isConvertedSuccessfully)
			{
                return "Direction incorrect format";
            }

            var positionsRecordUpdated = await positionsRecordListRepository
			    .GetAll()
			    .FirstOrDefaultAsync(record => record.Guid == recordId);

			if(positionsRecordUpdated == null)
			{
				return "Record not found";
			}

			positionsRecordUpdated.LastUpdated = dateTimeOfChange;

			var tradePosition = mapper.Map<TradePosition>(model, opt => 
			opt.AfterMap((src, dest) => 
			{
				dest.PositionsRecordListId = recordId;
				dest.PositionAddedOn = dateTimeOfChange;
				dest.RealizedProfitAndLoss = CalculationFormulas.CalculateFuturesPL(
                        parsedPositionDirection == PositionDirection.Long,
                        model.EntryPrice,
                        model.ExitPrice,
                        model.Quantity,
                        model.TickSize,
                        model.TickValue);
            }));


			var futuresContractGuid = (await futuresContractRepository
				.GetAllAsNoTracking()
                .Where(position => !position.IsDeleted)
                .FirstOrDefaultAsync(contract => contract.Name == model.ContractName))?.Guid;

			if(futuresContractGuid == null)
			{
				return "Futures Contract not found";
			}

			var futuresPosition = new FuturesPosition
			{
				FuturesContractId = futuresContractGuid.Value,
				TradePositionId = tradePosition.Guid,
				Direction = parsedPositionDirection
			};

			await tradePositionsRepository.AddAsync(tradePosition);

			await futuresPositionsRepository.AddAsync(futuresPosition);


			await tradePositionsRepository.SaveChangesAsync();

			await futuresPositionsRepository.SaveChangesAsync();

			await positionsRecordListRepository.SaveChangesAsync();


			return true;
		}

		public async Task<Result> ChangeFuturesPosition(
			Guid recordId,
			Guid positionGuid,
			Guid requesterGuid,
			ChangeFuturesPositionRequestModel model)
		{
			try
			{
				var dateTimeOfChange = DateTime.UtcNow;

                var isConvertedSuccessfully = 
								Enum.TryParse(model.Direction, out PositionDirection parsedPositionDirection);

                if (!isConvertedSuccessfully)
                {
                    return "Direction incorrect format";
                }

                var positionsRecordUpdated = await positionsRecordListRepository
                    .GetAll()
                    .FirstOrDefaultAsync(record => record.Guid == recordId);

				if(positionsRecordUpdated == null)
				{
					return "Record not found";
				}

				positionsRecordUpdated.LastUpdated = dateTimeOfChange;

				var tradePosition = await tradePositionsRepository
					.GetAll()
					.FirstOrDefaultAsync(position => position.Guid == positionGuid);

                if (tradePosition == null)
                {
                    return "Trade position not found";
                }

                var futuresPosition = await futuresPositionsRepository
                 .GetAllAsNoTracking()
                 .Where(position => !position.IsDeleted)
                 .Include(position => position.FuturesContract)
                 .FirstOrDefaultAsync(position => position.TradePositionId == positionGuid);

                if (futuresPosition == null)
                {
                    return "Futures position not found";
                }

				tradePosition.EntryPrice = model.EntryPrice;
				tradePosition.ExitPrice = model.ExitPrice;
				tradePosition.QuantitySize = model.Quantity;
				tradePosition.RealizedProfitAndLoss = CalculationFormulas.CalculateFuturesPL(
							parsedPositionDirection == PositionDirection.Long,
							model.EntryPrice,
							model.ExitPrice,
							model.Quantity,
							model.TickSize,
							model.TickValue);

				futuresPosition.Direction = parsedPositionDirection;


				tradePositionsRepository.Update(tradePosition);

				futuresPositionsRepository.Update(futuresPosition);


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

		public async Task<Result> DeleteFuturesPositions(
			Guid recordId,
			Guid positionGuid,
			Guid requesterGuid)
		{
			try
			{
				var deletePositionRecord = await positionsRecordListRepository
					.GetAllAsNoTracking()
                    .Where(position => !position.IsDeleted)
                    .FirstOrDefaultAsync(record => record.Guid == recordId);

                if (deletePositionRecord == null)
                {
                    return "Record not found";
                }

                if (deletePositionRecord.UserId != requesterGuid)
				{
                    return "User not owner";
                }

				var tradePosition = await tradePositionsRepository
                        .GetAllAsNoTracking()
						.Where(position => !position.IsDeleted)
                        .FirstAsync(p => p.Guid == positionGuid);

				tradePositionsRepository.Delete(tradePosition);

				await tradePositionsRepository.SaveChangesAsync();

				return true;
			}
			catch (UnauthorizedAccessException uae)
			{
				return uae.Message;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public CalculateFuturesPositionResponseModel CalculateFuturesPosition(CalculateFuturesPositionRequestModel model)
		{
            var isConvertedSuccessfully =
                                 Enum.TryParse(model.Direction, out PositionDirection parsedPositionDirection);

            if (!isConvertedSuccessfully)
            {
				throw new Exception("Incorrect direction format");
            }

            return new CalculateFuturesPositionResponseModel {
				ProfitLoss = CalculationFormulas.CalculateFuturesPL(
							parsedPositionDirection == PositionDirection.Long,
							model.EntryPrice,
							model.ExitPrice,
							model.Quantity,
							model.TickSize,
							model.TickValue),
				Ticks = CalculationFormulas.CalculateFuturesTicks(
							model.EntryPrice,
                            model.ExitPrice,
                            model.Quantity,
                            model.TickSize)
            };
        }

		public CalculateStocksPositionResponseModel CalculateStocksPosition(CalculateStocksPositionRequestModel model)
		{
            return new CalculateStocksPositionResponseModel
            {
                ProfitLoss = CalculationFormulas.CalculateStocksPL(
                            model.BuyPrice,
                            model.SellPrice,
                            model.NumberOfShares,
                            model.BuyCommission,
                            model.SellCommission),
            };
        }
	}
}
