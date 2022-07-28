using Profitable.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Profitable.Models.EntityModels.EntityBaseClass
{
    public class EntityBase : IDeletebleEntity
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
