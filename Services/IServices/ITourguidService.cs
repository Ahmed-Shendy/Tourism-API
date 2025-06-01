

namespace Tourism_Api.Services.IServices;

public interface ITourguidService
{
    Task<Result<TourguidProfile>> Profile(string id, CancellationToken cancellationToken = default);
    Task<Result> UpdateProfile(string id, TourguidUpdateProfile request, CancellationToken cancellationToken = default);
    Task<Result> RemoveTourist(string tourguidid, string Touristid, CancellationToken cancellationToken = default);
    Task<Result<TourguidPublicProfile>> PublicProfile(string id, CancellationToken cancellationToken = default);
    Task<Result> UploadPhoto(string id, IFormFile photo, CancellationToken cancellationToken = default);
    Task<Result> DeletePhoto(string id, CancellationToken cancellationToken = default);
    Task<Result> MoveToPlaces(string tourguidid, string placeName, CancellationToken cancellationToken = default);
    Task<Result> MoveToTripe(string tourguidid, string TripeName, CancellationToken cancellationToken = default);
    Task<Result> UpdateMaxTourists(string id, int maxTourists, CancellationToken cancellationToken = default);
    Task<Result> UpdateIsActive(string id, bool isActive, CancellationToken cancellationToken = default);
    Task<Result> CreateAcount(AddTourguidRequest request, CancellationToken cancellationToken = default);
    Task<(byte[] fileContent, string ContentType , string fileName)> DownloadAsync(string id, CancellationToken cancellationToken = default);
    
    Task<(byte[] fileContent, string ContentType, string fileName)> DownloadFilesAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<AllTripsResponse>> DisplayAllTrips(CancellationToken cancellationToken = default);
   // Task<Result<Dictionary<string, List<NotActiveTourguids>>>> DisplayTourguidInEachTrip(CancellationToken cancellationToken = default);
}
