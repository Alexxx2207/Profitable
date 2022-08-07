using Profitable.Models.EntityModels.EntityBaseClass;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profitable.Models.EntityModels
{
    public class Post : EntityBase
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
        }

        [Required]
        [ForeignKey("Author")]
        public Guid AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        public string? ImageURL { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Like> Likes { get; set; }

        public ICollection<PostTag> Tags { get; set; }
    }
}
