using System.ComponentModel;

namespace Domain.SoftDelete
{
    public interface ISoftDelete
    {
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}