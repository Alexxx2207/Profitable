namespace Profitable.Models.EntityModels
{
	using Profitable.Models.EntityModels.EntityBaseClass;
	using System.ComponentModel.DataAnnotations;

	public class Book : EntityBase
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Authors { get; set; }
	}
}
