using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.GlobalConstants;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Posts;
using Profitable.Services.Comments.Contracts;
using Profitable.Services.Search.Contracts;

namespace Profitable.Services.Search
{
	public class PostSearchService : IPostSearchService
	{
		private readonly IRepository<Post> postRepository;
		private readonly ICommentService commentService;
		private readonly IMapper mapper;

		public PostSearchService(
			IRepository<Post> postRepository,
			ICommentService commentService,
			IMapper mapper)
		{
			this.postRepository = postRepository;
			this.commentService = commentService;
			this.mapper = mapper;
		}

		public async Task<List<PostResponseModel>> GetMatchingPosts(string searchTerm, int page, int pageCount)
		{
			searchTerm = searchTerm.ToLower();

			var posts = await postRepository
				.GetAllAsNoTracking()
				.Include(p => p.Author)
				.Include(p => p.Likes)
				.Where(p =>
					!p.IsDeleted &&
					(p.Title.ToLower().Contains(searchTerm)
					||
					p.Content.ToLower().Contains(searchTerm)
					||
					(p.Author.FirstName + " " + p.Author.LastName).ToLower().Contains(searchTerm)))
				.Select(post => mapper.Map<PostResponseModel>(post))
                .Skip(page * pageCount)
                .Take(pageCount)
				.ToListAsync();

			foreach (var post in posts)
			{
				var commentsCount =
						await commentService.GetCommentsCountByPostAsync(Guid.Parse(post.Guid));

				post.CommentsCount = commentsCount;
			}

			return posts;
		}
	}
}
