namespace Profitable.Services.Search.Contracts
{
	using Profitable.Models.ResponseModels.Posts;

	public interface IPostSearchService
	{
		Task<List<PostResponseModel>> GetMatchingPosts(string searchTerm, int page, int pageCount);
	}
}
