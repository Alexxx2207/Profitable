using Profitable.Models.ResponseModels.Users;

namespace Profitable.Services.Search.Contracts
{
	public interface IUserSearchService
	{
		Task<List<UserDetailsResponseModel>> GetMatchingUsers(string searchTerm);
	}
}
