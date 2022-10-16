using Profitable.Models.ResponseModels.Posts;

namespace Profitable.Services.Search.Contracts
{
	public interface IPostSearchService
	{
		Task<List<PostResponseModel>> GetMatchingPosts(string searchTerm);
	}
}
