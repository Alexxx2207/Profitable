namespace Profitable.Models.Contracts
{
    public interface IDeletebleEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
