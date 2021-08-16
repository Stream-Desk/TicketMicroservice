using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Comments
{
    public interface ICommentsCollection
    {
        Task<Comment> CreateComment(Comment comment, CancellationToken cancellationToken = default);
        Task<List<Comment>> Comments(CancellationToken cancellationToken = default);
    }
}