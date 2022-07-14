﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profitable.Models.Contracts;

namespace Profitable.Models.EntityModels
{
    public class Post : EntityBase
    {
        public Post()
        {
            GUID = Guid.NewGuid().ToString();
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
        }

        [Required]
        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        public Trader Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }


        public ICollection<Comment> Comments { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}