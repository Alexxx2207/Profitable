using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profitable.Models.Contracts;


namespace Profitable.Models.EntityModels
{
    public class Comment : EntityBase
    {
        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        public Trader Author { get; set; }


        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public string Content { get; set; }


        [ForeignKey("Post")]
        public string PostId { get; set; }
        public Post Post { get; set; }

    }
}
