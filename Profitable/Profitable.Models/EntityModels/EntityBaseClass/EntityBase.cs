using Profitable.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Profitable.Models.EntityModels.EntityBaseClass
{
    public class EntityBase : IDeletebleEntity
    {
        public EntityBase()
        {
            Guid = Guid.NewGuid();
        }

        [Key]
        public Guid Guid { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

    }
}
