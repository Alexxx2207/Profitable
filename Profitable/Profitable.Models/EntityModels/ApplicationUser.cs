namespace Profitable.Models.EntityModels
{
	using Microsoft.AspNetCore.Identity;
	using Profitable.Models.Contracts;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class ApplicationUser : IdentityUser<Guid>, IDeletebleEntity
	{
		public ApplicationUser()
		{
			Id = Guid.NewGuid();
			Roles = new HashSet<IdentityUserRole<Guid>>();
			Claims = new HashSet<IdentityUserClaim<Guid>>();
			Logins = new HashSet<IdentityUserLogin<Guid>>();
		}

		public ApplicationUser(string userName) : base(userName)
		{
		}

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		public string? Description { get; set; }

		public string? ProfilePictureURL { get; set; }

		[ForeignKey("Organization")]
		public Guid? OrganizationId { get; set; }
		public Organization? Organization { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? DeletedOn { get; set; }

		public virtual ICollection<IdentityUserRole<Guid>> Roles { get; set; }

		public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }

		public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
	}
}