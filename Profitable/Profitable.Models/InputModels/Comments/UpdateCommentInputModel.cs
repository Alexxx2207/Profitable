﻿using Profitable.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.InputModels.Comments
{
    public class UpdateCommentInputModel
    {
        public string Guid { get; set; }

        public string Content { get; set; }
    }
}