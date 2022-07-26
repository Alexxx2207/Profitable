using System.ComponentModel.DataAnnotations;

namespace Profitable.Models.Contracts
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
