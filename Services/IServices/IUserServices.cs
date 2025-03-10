using Tourism_Api.Entity.Comment;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.user;

namespace Tourism_Api.Services.IServices;

public interface IUserServices
{
    Task<Result<UserRespones>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> AddComment(string UserId , AddComment request, CancellationToken cancellationToken = default);
    Task<Result> DeleteComment(string UserId, int CommentId, CancellationToken cancellationToken = default);
    Task<Result> UpdateComment(string UserId, UpdateComment request, CancellationToken cancellationToken = default);
    Task<Result> Addrate(string UserId, AddRate request, CancellationToken cancellationToken = default);
}
