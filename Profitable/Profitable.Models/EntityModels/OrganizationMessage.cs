using Profitable.Common.GlobalConstants;
using Profitable.Models.EntityModels.EntityBaseClass;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profitable.Models.EntityModels
{
	public class OrganizationMessage : EntityBase
	{
		public OrganizationMessage()
		{
			SentOn = DateTime.UtcNow;
		}

		[ForeignKey("Sender")]
		public Guid SenderId { get; set; }
		public ApplicationUser Sender { get; set; }

		[ForeignKey("Organization")]
		public Guid OrganizationId { get; set; }
		public Organization Organization { get; set; }

		[MaxLength(GlobalDatabaseConstants.MESSAGE_MAX_LENGTH)]
		public string Message { get; set; }

		public DateTime SentOn { get; set; }
	}
}
