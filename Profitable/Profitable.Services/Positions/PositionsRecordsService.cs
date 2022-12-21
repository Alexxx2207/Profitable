namespace Profitable.Services.Positions
{
	using AutoMapper;
	using Microsoft.EntityFrameworkCore;
	using Profitable.Common.Enums;
	using Profitable.Common.GlobalConstants;
	using Profitable.Common.Models;
	using Profitable.Common.Services;
	using Profitable.Data.Repository.Contract;
	using Profitable.Models.EntityModels;
	using Profitable.Models.ResponseModels.Positions.Records;
	using Profitable.Services.Positions.Contracts;

	public class PositionsRecordsService : IPositionsRecordsService
	{
		private readonly IRepository<PositionsRecordList> repository;
		private readonly IMapper mapper;

		public PositionsRecordsService(
			IRepository<PositionsRecordList> repository,
			IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		public async Task<Result> AddPositionsRecordList(
			Guid userGuid,
			string recordName,
			string instrumentGroup)
		{
			try
			{
				Enum.TryParse(instrumentGroup, out InstrumentGroup parsedInstrumentGroup);

				await repository.AddAsync(new PositionsRecordList
				{
					UserId = userGuid,
					Name = recordName,
					InstrumentGroup = parsedInstrumentGroup,
				});

				await repository.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				return e.Message;
			}

		}

		public async Task<Result> ChangeNamePositionsRecordList(
			Guid recordGuid,
			string recordName,
			Guid requesterGuid)
		{
			try
			{
				var recordToUpdate = await repository
				.GetAllAsNoTracking()
				.Where(post => !post.IsDeleted)
				.FirstOrDefaultAsync(entity => entity.Guid == recordGuid);

				if (recordToUpdate == null)
				{
					return GlobalServicesConstants.EntityDoesNotExist("Positions record");
				}

				if (recordToUpdate.UserId != requesterGuid)
				{
					return GlobalServicesConstants.RequesterNotOwnerMessage;
				}

				recordToUpdate.Name = recordName;

				repository.Update(recordToUpdate);

				await repository.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public async Task<Result> DeletePositionsRecordList(Guid recordGuid, Guid requesterGuid)
		{
			try
			{
				var record = await repository
				.GetAllAsNoTracking()
				.FirstOrDefaultAsync(entity => entity.Guid == recordGuid);

				if (record == null)
				{
					return GlobalServicesConstants.EntityDoesNotExist("Positions record");
				}

				if (record.UserId != requesterGuid)
				{
					return GlobalServicesConstants.RequesterNotOwnerMessage;
				}

				repository.Delete(record);

				await repository.SaveChangesAsync();

				return true;

			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public IEnumerable<string> GetPositionsRecordsOrderTypes()
		{
			return Enum.GetValues<OrderPositionsRecordBy>()
					.Select(type => StringManipulations.DivideCapitalizedStringToWords(type.ToString()));
		}

		public async Task<List<UserPositionsRecordResponseModel>> GetUserRecordsAsync(
			Guid userGuid,
			int page,
			int pageCount,
			OrderPositionsRecordBy OrderPositionsRecordBy)
		{
			var records = await repository
				.GetAllAsNoTracking()
				.Include(r => r.Positions.Where(position => !position.IsDeleted))
				.Where(r =>
					!r.IsDeleted &&
					r.UserId == userGuid)
				.ToListAsync();

			if (OrderPositionsRecordBy == OrderPositionsRecordBy.Date)
			{
				records = records.OrderBy(r => r.LastUpdated).ToList();
			}
			else if (OrderPositionsRecordBy == OrderPositionsRecordBy.DateDescending)
			{
				records = records.OrderByDescending(r => r.LastUpdated).ToList();
			}
			else if (OrderPositionsRecordBy == OrderPositionsRecordBy.Name)
			{
				records = records.OrderBy(r => r.Name).ToList();
			}
			else if (OrderPositionsRecordBy == OrderPositionsRecordBy.NameDescending)
			{
				records = records.OrderByDescending(r => r.Name).ToList();
			}
			else if (OrderPositionsRecordBy == OrderPositionsRecordBy.Positions)
			{
				records = records.OrderBy(r => r.Positions.Count).ToList();
			}
			else if (OrderPositionsRecordBy == OrderPositionsRecordBy.PositionsDescending)
			{
				records = records.OrderByDescending(r => r.Positions.Count).ToList();
			}

			var result = records
				.Skip(page * pageCount)
				.Take(pageCount)
				.Select(comment => mapper.Map<UserPositionsRecordResponseModel>(comment))
				.ToList();

			return result;
		}

		public async Task<List<UserPositionsRecordResponseModel>> GetUserRecordsByInstrumentGroupAsync(
			Guid userGuid,
			int page,
			int pageCount,
			InstrumentGroup instrumentGroup)
		{
			var records = await repository
				.GetAllAsNoTracking()
				.Include(r => r.Positions.Where(position => !position.IsDeleted))
				.Where(r =>
					!r.IsDeleted &&
					r.InstrumentGroup == instrumentGroup &&
					r.UserId == userGuid)
				.OrderByDescending(r => r.LastUpdated)
				.ToListAsync();

			var result = records
				.Skip(page * pageCount)
				.Take(pageCount)
				.Select(comment => mapper.Map<UserPositionsRecordResponseModel>(comment))
				.ToList();

			return result;
		}
	}
}
