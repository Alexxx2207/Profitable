namespace Profitable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    public class Trader : IdentityUser
    {
        public Trader()
        {
            Id = Guid.NewGuid().ToString();
            Roles = new HashSet<IdentityUserRole<string>>();
            Claims = new HashSet<IdentityUserClaim<string>>();
            Logins = new HashSet<IdentityUserLogin<string>>();
        }

        public bool IsDeleted { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? ProfilePicture { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}