namespace Profitable.Services.Posts.Contracts
{
	using Profitable.Common.Models;
	using Profitable.Models.RequestModels.Posts;
	using Profitable.Models.ResponseModels.Posts;
	public interface IPostService
	{
		Task<PostResponseModel> GetPostByGuidAsync(Guid guid, string? loggedInUserEmail);

		Task<List<PostResponseModel>> GetPostsByPageAsync(int page, int pageCount, string? loggedInUserEmail);

		Task<List<PostResponseModel>> GetPostsByUserAsync(Guid userId, int page, int pageCount);

		Task<int> ManagePostLikeAsync(Guid authorId, Guid postGuid);

		Task<Result> AddPostAsync(Guid authorId, AddPostRequestModel newPost);

		Task<Result> DeletePostAsync(Guid guid, Guid requesterGuid);

		Task<Result> UpdatePostAsync(Guid postGuid, UpdatePostRequestModel newPost, Guid requesterGuid);
	}
}
