namespace Profitable.Models.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;
    using Profitable.GlobalConstants;
    using Profitable.Models.Contracts;

    public class ApplicationUser : IdentityUser, IDeletebleEntity
    { 
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Roles = new HashSet<IdentityUserRole<string>>();
            Claims = new HashSet<IdentityUserClaim<string>>();
            Logins = new HashSet<IdentityUserLogin<string>>();

            Lists = new HashSet<List>();

            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();

        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public ImageTypes? ImageType { get; set; }

        public string? ProfilePictureURL { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }


        public virtual ICollection<List> Lists { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

    }
}