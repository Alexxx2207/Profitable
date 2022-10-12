using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.Enums;
using Profitable.Common.Models;
using Profitable.Common.Services;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Positions;
using Profitable.Services.Positions.Contracts;

namespace Profitable.Services.Positions
{
	public class PositionsRecordsService : IPositionsRecordsService
	{
		private readonly IRepository<PositionsRecordList> repository;
		private readonly IMapper mapper;

		public PositionsRecordsService(IRepository<PositionsRecordList> repository, IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		public async Task<Result> AddPositionsRecordList(Guid userGuid, string positionName)
		{
			try
			{
				await repository.AddAsync(new PositionsRecordList
				{
					UserId = userGuid,
					Name = positionName,
				});

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

		public async Task<List<UserPositionsRecordResponseModel>> GetUserPositionsRecordsAsync(
			Guid userGuid,
			int page,
			int pageCount,
			OrderPositionsRecordBy OrderPositionsRecordBy)
		{
			var records = await repository
				.GetAllAsNoTracking()
				.Where(r => r.UserId == userGuid)
				.Include(r => r.Positions)
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
	}
}
