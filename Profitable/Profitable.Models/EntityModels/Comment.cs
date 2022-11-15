namespace Profitable.Models.EntityModels
{
    using Profitable.Models.EntityModels.EntityBaseClass;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment : EntityBase
    {
        [ForeignKey("Author")]
        public Guid AuthorId { get; set; }
        public ApplicationUser Author { get; set; }


        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public string Content { get; set; }


        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public Post Post { get; set; }

    }
}
