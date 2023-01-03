namespace Profitable.Services.Journals.Contracts
{
	using Profitable.Common.Models;
	using Profitable.Models.RequestModels.Journals;
	using Profitable.Models.ResponseModels.Journals;

	public interface IJournalService
	{
		Task<List<JournalResponseModel>> GetUserJournals(Guid userId, int page, int pageCount);

		Task<JournalResponseModel> GetUserJournal(Guid userId, Guid journalId);

		Task<Result> AddUserJournals(Guid userId, AddJournalRequestModel model);

		Task<Result> UpdateUserJournals(Guid userId, UpdateJournalRequestModel model);

		Task<Result> DeleteUserJournals(Guid userId, Guid journalId);
	}
}
