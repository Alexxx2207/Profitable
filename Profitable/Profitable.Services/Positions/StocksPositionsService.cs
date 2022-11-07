using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.Enums;
using Profitable.Common.GlobalConstants;
using Profitable.Common.Models;
using Profitable.Common.Services;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Positions.Stocks;
using Profitable.Models.ResponseModels.Positions.Stocks;
using Profitable.Services.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Positions
{
    public class StocksPositionsService : IStocksPositionsService
    {
        private readonly IRepository<TradePosition> tradePositionsRepository;
        private readonly IRepository<StocksPosition> stocksPositionsRepository;
        private readonly IRepository<PositionsRecordList> positionsRecordListRepository;
        private readonly IMapper mapper;

        public StocksPositionsService(
            IRepository<TradePosition> tradePositionsRepository,
            IRepository<StocksPosition> stocksPositionsRepository,
            IRepository<PositionsRecordList> positionsRecordListRepository,
            IMapper mapper)
        {
            this.tradePositionsRepository = tradePositionsRepository;
            this.stocksPositionsRepository = stocksPositionsRepository;
            this.positionsRecordListRepository = positionsRecordListRepository;
            this.mapper = mapper;
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

        public async Task<List<StocksPositionResponseModel>> GetStocksPositions(
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

            var results = new List<StocksPositionResponseModel>();

            foreach (var position in tradePositions)
            {
                var stocksPosition = await stocksPositionsRepository
                    .GetAllAsNoTracking()
                    .Where(position => !position.IsDeleted)
                    .FirstOrDefaultAsync(p => p.TradePositionId == position.Guid);

                if (stocksPosition == null)
                {
                    continue;
                }

                var responseModel = mapper.Map(
                    stocksPosition,
                    mapper.Map<StocksPositionResponseModel>(position)
                );

                results.Add(responseModel);
            }

            return results;
        }

        public async Task<StocksPositionResponseModel> GetStocksPositionByGuid(Guid positionGuid)
        {
            var tradePosition = await tradePositionsRepository
                .GetAllAsNoTracking()
                .Where(position => !position.IsDeleted)
                .FirstOrDefaultAsync(position => position.Guid == positionGuid);

            var stocksPosition = await stocksPositionsRepository
                .GetAllAsNoTracking()
                .Where(position => !position.IsDeleted)
                .FirstOrDefaultAsync(position => position.TradePositionId == positionGuid);

            if (tradePosition == null)
            {
                throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Trade position"));
            }
            else if (stocksPosition == null)
            {
                throw new Exception(GlobalServicesConstants.EntityDoesNotExist("stocks position"));
            }

            var responseModel = mapper.Map(
                    stocksPosition,
                    mapper.Map<StocksPositionResponseModel>(tradePosition)
                );

            return responseModel;
        }

        public async Task<Result> AddStocksPositions(
            Guid recordId,
            AddStocksPositionRequestModel model,
            Guid requesterGuid)
        {
            try
            {
                var dateTimeOfChange = DateTime.UtcNow;

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
                        dest.RealizedProfitAndLoss = CalculationFormulas.CalculateStocksPL(
                                model.EntryPrice,
                                model.ExitPrice,
                                model.QuantitySize,
                                model.BuyCommission,
                                model.SellCommission);
                    }));


                var stocksPosition = mapper.Map<StocksPosition>(model, opt =>
                   opt.AfterMap((src, dest) =>
                   {
                       dest.TradePositionId = tradePosition.Guid;
                   }));

                await tradePositionsRepository.AddAsync(tradePosition);

                await stocksPositionsRepository.AddAsync(stocksPosition);


                await tradePositionsRepository.SaveChangesAsync();

                await stocksPositionsRepository.SaveChangesAsync();

                await positionsRecordListRepository.SaveChangesAsync();


                return true;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<Result> ChangeStocksPosition(
            Guid recordId,
            Guid positionGuid,
            Guid requesterGuid,
            ChangeStocksPositionRequestModel model)
        {
            try
            {
                var dateTimeOfChange = DateTime.UtcNow;

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

                var stocksPosition = await stocksPositionsRepository
                 .GetAllAsNoTracking()
                 .Where(position => !position.IsDeleted)
                 .FirstOrDefaultAsync(position => position.TradePositionId == positionGuid);

                if (stocksPosition == null)
                {
                    return GlobalServicesConstants.EntityDoesNotExist("stocks position");
                }

                tradePosition.EntryPrice = model.EntryPrice;
                tradePosition.ExitPrice = model.ExitPrice;
                tradePosition.QuantitySize = model.QuantitySize;
                tradePosition.RealizedProfitAndLoss = CalculationFormulas.CalculateStocksPL(
                            model.EntryPrice,
                            model.ExitPrice,
                            model.QuantitySize,
                            model.BuyCommission,
                            model.SellCommission);

                tradePositionsRepository.Update(tradePosition);

                stocksPositionsRepository.Update(stocksPosition);


                await tradePositionsRepository.SaveChangesAsync();

                await stocksPositionsRepository.SaveChangesAsync();

                await positionsRecordListRepository.SaveChangesAsync();


                return true;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<Result> DeleteStocksPositions(
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

                if (tradePosition == null)
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
    }
}
