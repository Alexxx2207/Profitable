namespace Profitable.Models.EntityModels
{
	using Profitable.Models.EntityModels.EntityBaseClass;
	using System.ComponentModel.DataAnnotations;

	public class FuturesContract : EntityBase
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public double TickSize { get; set; }

		[Required]
		public double TickValue { get; set; }
	}
}
