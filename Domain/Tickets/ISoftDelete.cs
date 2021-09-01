namespace Domain.Tickets
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}