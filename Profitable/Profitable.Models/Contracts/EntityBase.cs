using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.Contracts
{
    public class EntityBase
    {
        public EntityBase()
        {
            GUID = Guid.NewGuid().ToString();
        }

        [Key]
        public string GUID { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
