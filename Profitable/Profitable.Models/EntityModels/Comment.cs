using Profitable.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Profitable.Models.EntityModels
{
    public class Comment : EntityBase
    {

        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }


        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public string Content { get; set; }


        [ForeignKey("Post")]
        public string PostId { get; set; }
        public Post Post { get; set; }

    }
}
