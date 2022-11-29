namespace Profitable.Services.Search.Contracts
{
	using Profitable.Models.ResponseModels.Users;

	public interface IUserSearchService
	{
		Task<List<UserDetailsResponseModel>> GetMatchingUsers(string searchTerm, int page, int pageCount);
	}
}
