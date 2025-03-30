using Tourism_Api.Entity.Tourguid;

namespace Tourism_Api.Services.IServices;

public interface ITourguidService
{
    Task<Result<TourguidProfile>> Profile(string id, CancellationToken cancellationToken = default);
    Task<Result> UpdateProfile(string id, TourguidUpdateProfile request, CancellationToken cancellationToken = default);
    Task<Result> RemoveTourist(string tourguidid, string Touristid, CancellationToken cancellationToken = default);
    Task<Result<TourguidPublicProfile>> PublicProfile(string id, CancellationToken cancellationToken = default);
}
