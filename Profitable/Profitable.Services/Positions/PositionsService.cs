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
					.FirstOrDefaultAsync(p => p.TradePositionId == position.Guid);

				if(futuresPosition == null)
				{
					continue;
				}

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
		
		public async Task<PositionResponseModel> GetFuturesPositionByGuid(Guid positionGuid)
		{
			var tradePosition = await tradePositionsRepository
				.GetAllAsNoTracking()
				.FirstOrDefaultAsync(position => position.Guid == positionGuid);
			
			var futuresPosition = await futuresPositionsRepository
				.GetAllAsNoTracking()
				.Include(position => position.FuturesContract)
				.FirstOrDefaultAsync(position => position.TradePositionId == positionGuid);

			if(tradePosition == null )
			{
				throw new Exception("Trade position not found");
			}
			else if(futuresPosition == null)
			{
                throw new Exception("Futures position not found");
            }

            string parsedDirection = Enum.GetName(typeof(PositionDirection), futuresPosition.Direction);

            var result = new PositionResponseModel
            {
                Guid = tradePosition.Guid.ToString(),
                PositionAddedOn = tradePosition.PositionAddedOn.ToString("F"),
                ContractName = futuresPosition.FuturesContract.Name,
                Direction = parsedDirection,
                EntryPrice = tradePosition.EntryPrice.ToString(),
                ExitPrice = tradePosition.ExitPrice.ToString(),
                Quantity = tradePosition.QuantitySize.ToString(),
                TickSize = futuresPosition.FuturesContract.TickSize.ToString(),
                TickValue = futuresPosition.FuturesContract.TickValue.ToString(),
                PositionPAndL = tradePosition.RealizedProfitAndLoss.ToString()
            };

            return result;
		}

		public async Task<Result> AddFuturesPositions(Guid recordId, AddFuturesPositionRequestModel model)
		{
			var dateTimeOfChange = DateTime.UtcNow;

			var isConvertedSuccessfully = Enum.TryParse(model.Direction, out PositionDirection parsedPositionDirection);

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

			var tradePosition = new TradePosition
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
				.FirstOrDefaultAsync(contract => contract.Name == model.ContractName)).Guid;

			if(futuresContractGuid == null)
			{
				return "Futures Contract not found";
			}

			var futuresPosition = new FuturesPosition
			{
				FuturesContractId = futuresContractGuid,
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

        public async Task<Result> ChangeFuturesPosition(Guid recordId, Guid positionGuid, Guid requesterGuid, ChangeFuturesPositionRequestModel model)
        {
            try
            {
                var dateTimeOfChange = DateTime.UtcNow;

                var isConvertedSuccessfully = Enum.TryParse(model.Direction, out PositionDirection parsedPositionDirection);

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
                 .Include(position => position.FuturesContract)
                 .FirstOrDefaultAsync(position => position.TradePositionId == positionGuid);

                if (futuresPosition == null)
                {
                    return "Futures position not found";
                }

                string parsedDirection = Enum.GetName(typeof(PositionDirection), futuresPosition.Direction);

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

        public async Task<Result> DeleteFuturesPositions(Guid recordId, Guid positionGuid, Guid requesterGuid)
		{
			try
			{
				var deletePositionRecord = await positionsRecordListRepository
					.GetAllAsNoTracking()
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
						.FirstOrDefaultAsync(p => p.Guid == positionGuid);

                if (deletePositionRecord == null)
                {
                    return "Trade position not found";
                }

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
	}
}
