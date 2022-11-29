namespace Profitable.Models.EntityModels
{
	using Profitable.Models.EntityModels.EntityBaseClass;
	using System.ComponentModel.DataAnnotations;

	public class Exchange : EntityBase
	{
		[Required]
		public string Name { get; set; }
	}
}
