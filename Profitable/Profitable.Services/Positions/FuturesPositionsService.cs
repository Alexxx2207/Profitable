using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.Enums;
using Profitable.Common.GlobalConstants;
using Profitable.Common.Models;
using Profitable.Common.Services;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Positions.Futures;
using Profitable.Models.ResponseModels.Positions.Futures;
using Profitable.Services.Positions.Contracts;

namespace Profitable.Services.Positions
{
	public class FuturesPositionsService : IFuturesPositionsService
    {
		private readonly IRepository<TradePosition> tradePositionsRepository;
		private readonly IRepository<FuturesPosition> futuresPositionsRepository;
		private readonly IRepository<FuturesContract> futuresContractRepository;
		private readonly IRepository<PositionsRecordList> positionsRecordListRepository;
		private readonly IMapper mapper;

		public FuturesPositionsService(
			IRepository<TradePosition> tradePositionsRepository,
			IRepository<FuturesPosition> futuresPositionsRepository,
			IRepository<FuturesContract> futuresContractRepository,
			IRepository<PositionsRecordList> positionsRecordListRepository,
			IMapper mapper)
		{
			this.tradePositionsRepository = tradePositionsRepository;
			this.futuresPositionsRepository = futuresPositionsRepository;
			this.futuresContractRepository = futuresContractRepository;
			this.positionsRecordListRepository = positionsRecordListRepository;
			this.mapper = mapper;
		}

		public async Task<List<FuturesPositionResponseModel>> GetFuturesPositions(
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
				.OrderBy(p => p.PositionAddedOn)
				.ToListAsync();

			var results = new List<FuturesPositionResponseModel>();

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
					mapper.Map<FuturesPositionResponseModel>(
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

		public async Task<FuturesPositionResponseModel> GetFuturesPositionByGuid(Guid positionGuid)
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
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Trade position"));
			}
			else if(futuresPosition == null)
			{
                throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Futures position"));
            }

			string parsedDirection = Enum.GetName(typeof(PositionDirection), futuresPosition.Direction);

            var responseModel = mapper.Map(
                    futuresPosition.FuturesContract,
                    mapper.Map<FuturesPositionResponseModel>(
                        tradePosition,
                        opt => opt.AfterMap((src, dest) =>
                        {
                            dest.Direction = parsedDirection;
                        })
                    )
                );

            return responseModel;
		}

		public async Task<Result> AddFuturesPositions(
			Guid recordId,
			AddFuturesPositionRequestModel model,
			Guid requesterGuid)
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

                if (positionsRecordUpdated == null)
                {
                    return GlobalServicesConstants.EntityDoesNotExist("Positions record");
                }

                if (positionsRecordUpdated.UserId != requesterGuid)
                {
                    return GlobalServicesConstants.RequesterNotOwnerMesssage;
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

                if (futuresContractGuid == null)
                {
                    return GlobalServicesConstants.EntityDoesNotExist("Futures contract");
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
			catch (Exception e)
			{
				return e.Message;
			}
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

                if (positionsRecordUpdated == null)
                {
                    return GlobalServicesConstants.EntityDoesNotExist("Positions record");
                }

                if (positionsRecordUpdated.UserId != requesterGuid)
                {
                    return GlobalServicesConstants.RequesterNotOwnerMesssage;
                }

                positionsRecordUpdated.LastUpdated = dateTimeOfChange;

				var tradePosition = await tradePositionsRepository
					.GetAll()
					.FirstOrDefaultAsync(position => position.Guid == positionGuid);

                if (tradePosition == null)
                {
					return GlobalServicesConstants.EntityDoesNotExist("Trade position");
                }

                var futuresPosition = await futuresPositionsRepository
                 .GetAllAsNoTracking()
                 .Where(position => !position.IsDeleted)
                 .Include(position => position.FuturesContract)
                 .FirstOrDefaultAsync(position => position.TradePositionId == positionGuid);

                if (futuresPosition == null)
                {
					return GlobalServicesConstants.EntityDoesNotExist("Futures position");
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
                    return GlobalServicesConstants.EntityDoesNotExist("Positions record");
                }

                if (deletePositionRecord.UserId != requesterGuid)
				{
                    return GlobalServicesConstants.RequesterNotOwnerMesssage;
                }

				var tradePosition = await tradePositionsRepository
                        .GetAllAsNoTracking()
						.Where(position => !position.IsDeleted)
                        .FirstOrDefaultAsync(p => p.Guid == positionGuid);

				if(tradePosition == null)
				{
                    return GlobalServicesConstants.EntityDoesNotExist("Trade position");
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
	}
}
