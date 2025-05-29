using Tourism_Api.Entity.Comment;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.user;

namespace Tourism_Api.Services.IServices;

public interface IUserServices
{
    Task<Result<UserRespones>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> AddComment(string UserId , AddComment request, CancellationToken cancellationToken = default);
    Task<Result> DeleteComment(string UserId, int CommentId, CancellationToken cancellationToken = default);
    Task<Result> UpdateComment(string UserId, UpdateComment request, CancellationToken cancellationToken = default);
    Task<Result> Addrate(string UserId, AddRate request, CancellationToken cancellationToken = default);
    Task<Result> ReservationTourguid(string UserId, string TourguidId, CancellationToken cancellationToken = default);
    Task<Result> CancelReservationTourguid(string UserId, CancellationToken cancellationToken = default);
    Task<Result<Tourguids>> DisplayReservationTourguid(string UserId, CancellationToken cancellationToken = default);
    Task<Result<Profile>> UserProfile(string UserId, CancellationToken cancellationToken = default);
    Task<Result<Public_Profile>> PublicProfile(string UserId, CancellationToken cancellationToken = default);
    Task<Result> UpdateProfile(string UserId, ProfileUpdate request, CancellationToken cancellationToken = default);
    Task<Result> AddOrRemoveFavoritePlace(string UserId, string PlaceName, CancellationToken cancellationToken = default);
    Task<Result> AddTourguidRate(string UserId, AddTourguidRate request, CancellationToken cancellationToken = default);
    Task<Result> SendContactUsProblem(string userId, UserProblem Problem, CancellationToken cancellationToken = default);
}
