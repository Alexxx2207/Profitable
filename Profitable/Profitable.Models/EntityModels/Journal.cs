namespace Profitable.Models.EntityModels
{
	using Profitable.Models.EntityModels.EntityBaseClass;

	public class Journal : EntityBase
	{
		public Journal()
		{
			PostedOn = DateTime.UtcNow;
		}

		public Guid UserId { get; set; }

		public ApplicationUser User { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime PostedOn { get; set; }
	}
}
