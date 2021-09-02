using System.ComponentModel;

namespace Domain.SoftDelete
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; } 
    }
}