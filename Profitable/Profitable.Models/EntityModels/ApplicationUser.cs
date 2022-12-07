namespace Profitable.Models.EntityModels
{
    using Microsoft.AspNetCore.Identity;
    using Profitable.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationUser : IdentityUser<Guid>, IDeletebleEntity
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
            Roles = new HashSet<IdentityUserRole<string>>();
            Claims = new HashSet<IdentityUserClaim<string>>();
            Logins = new HashSet<IdentityUserLogin<string>>();

            Lists = new HashSet<List>();

        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? Description { get; set; }

        public string? ProfilePictureURL { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }


        public virtual ICollection<List> Lists { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

    }
}