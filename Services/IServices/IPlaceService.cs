using Tourism_Api.Entity.Places;
using Tourism_Api.Pagnations;

namespace Tourism_Api.Services.IServices;

public interface IPlaceService
{
    Task<Result<All_Places>> DisplayAllPlaces(CancellationToken cancellationToken = default);
    Task<PaginatedList<ALLPlaces>> DisplayAllPlacesByPagnation(RequestFilters requestFilters, CancellationToken cancellationToken = default);
    Task<Result<PlacesDetails>> PlacesDetails(string userid, string name, CancellationToken cancellationToken = default);
    Task<Result<List<string>>> AllPlacesName(CancellationToken cancellationToken);
    Task<Result<List<ALLPlaces>>> SearchForPlace(string name, CancellationToken cancellationToken = default);
}
