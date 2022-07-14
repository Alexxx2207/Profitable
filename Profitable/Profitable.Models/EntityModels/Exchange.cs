using Profitable.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.EntityModels
{
    public class Exchange : EntityBase
    {
        public Exchange()
        {
            GUID = Guid.NewGuid().ToString();
        }

        [Required]
        public string Name { get; set; }
    }
}
