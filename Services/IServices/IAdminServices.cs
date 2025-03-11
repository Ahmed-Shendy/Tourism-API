using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Tourguid;

namespace Tourism_Api.Services.IServices;

public interface IAdminServices
{
    Task<Result<PlacesDetails>> AddPlace(AddPlaceRequest request, CancellationToken cancellationToken = default);
    Task<Result> AddTourguid(AddTourguidRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdatePlace(AddPlaceRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeletePlace(string name, CancellationToken cancellationToken = default);
    Task<Result> DeleteTourguid(string id, CancellationToken cancellationToken = default);
    Task<Result> AddTourguidPlace(string tourguidId, string placeName, CancellationToken cancellationToken = default);
    Task<Result> DeleteTourguidPlace(string tourguidId, string placeName, CancellationToken cancellationToken = default);
    Task<Result<List<AllTourguids>>> DisplayAllTourguid(CancellationToken cancellationToken = default);

}
