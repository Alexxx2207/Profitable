namespace Profitable.Services.Journals
{
	using AutoMapper;
	using Microsoft.EntityFrameworkCore;
	using Profitable.Common.GlobalConstants;
	using Profitable.Common.Models;
	using Profitable.Data.Repository.Contract;
	using Profitable.Models.EntityModels;
	using Profitable.Models.RequestModels.Journals;
	using Profitable.Models.ResponseModels.Journals;
	using Profitable.Services.Journals.Contracts;

	public class JournalService : IJournalService
	{
		private readonly IMapper mapper;
		private readonly IRepository<Journal> repository;

		public JournalService(
			IMapper mapper,
			IRepository<Journal> repository)
		{
			this.mapper = mapper;
			this.repository = repository;
		}

		public async Task<Result> AddUserJournals(
			Guid userId,
			AddJournalRequestModel model)
		{
			try
			{
				var journal = new Journal()
				{
					UserId = userId,
					Title = model.Title,
					Content = model.Content,
				};

				await repository.AddAsync(journal);

				await repository.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				return e.Message;
			}

		}

		public async Task<Result> DeleteUserJournals(Guid userId, Guid journalId)
		{
			try
			{
				var journal = await repository
				.GetAll()
					.FirstOrDefaultAsync(j => j.UserId == userId && j.Guid == journalId);

				if (journal == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Journal"));
				}

				repository.Delete(journal);

				await repository.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public async Task<Result> UpdateUserJournals(
			Guid userId,
			UpdateJournalRequestModel model)
		{
			try
			{
				var journal = await repository
				.GetAllAsNoTracking()
					.FirstOrDefaultAsync(j => j.UserId == userId && j.Guid == model.JournalId);

				if (journal == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Journal"));
				}

				var newJournal = mapper.Map<Journal>(journal);

				newJournal.Guid = journal.Guid;

				repository.Update(newJournal);

				await repository.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public async Task<List<JournalResponseModel>> GetUserJournals(
			Guid userId,
			int page,
			int pageCount)
		{
			var journals = await repository
				.GetAllAsNoTracking()
				.Where(j => !j.IsDeleted && j.UserId == userId)
				.OrderByDescending(j => j.PostedOn)
				.Skip(page * pageCount)
				.Take(pageCount)
				.ToListAsync();

			return mapper.Map<List<JournalResponseModel>>(journals);
		}

		public async Task<JournalResponseModel> GetUserJournal(Guid userId, Guid journalId)
		{
			var journal = await repository
				.GetAllAsNoTracking()
				.FirstOrDefaultAsync(j => !j.IsDeleted &&
				j.UserId == userId && j.Guid == journalId);

			return mapper.Map<JournalResponseModel>(journal);
		}
	}
}
