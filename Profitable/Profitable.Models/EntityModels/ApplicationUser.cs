namespace Profitable.Models.EntityModels
{
	using Profitable.Common.Enums;
	using Profitable.Models.EntityModels.EntityBaseClass;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class ApplicationUser : EntityBase
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		[Required]
		public string Salt { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		public string? Description { get; set; }

		public string? ProfilePictureURL { get; set; }

		[ForeignKey("Organization")]
		public Guid? OrganizationId { get; set; }
		public Organization? Organization { get; set; }

		public UserOrganizationsRoles OrganizationRole { get; set; }
	}
}