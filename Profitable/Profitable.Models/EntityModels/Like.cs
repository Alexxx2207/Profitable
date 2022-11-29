namespace Profitable.Models.EntityModels
{
	using Profitable.Models.EntityModels.EntityBaseClass;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Like : EntityBase
	{
		[ForeignKey("Author")]
		public Guid AuthorId { get; set; }
		public ApplicationUser Author { get; set; }


		[ForeignKey("Post")]
		public Guid PostId { get; set; }
		public Post Post { get; set; }
	}
}
