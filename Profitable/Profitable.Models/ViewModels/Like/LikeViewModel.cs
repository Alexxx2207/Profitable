using Profitable.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.ViewModels.Like
{
    public class LikeViewModel
    {
        public string GUID { get; set; }

        public Trader Trader { get; set; }
    }
}
