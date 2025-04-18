using Tourism_Api.Entity.TypeOfTourism;
using Tourism_Api.Pagnations;

namespace Tourism_Api.Services.IServices
{
    public interface ITypeOfTourismService
    {
        Task<Result<IEnumerable<TypeOfTourismResponse>>> GetAllTypeOfTourismAsync(CancellationToken cancellationToken);
        Task<Result<PaginatedList<TypeOfTourismALLPlaces>>> GetTypeOfTourismAndPlacesAsync
              (RequestFiltersScpical requestFilters, CancellationToken cancellationToken);
        Task<Result<TypeOfTourismAndPlacesResponse>> GetTypeOfTourismAndPlaces(String Name , CancellationToken cancellationToken);
        Task<Result<List<string>>> AllTypeOfTourismName(CancellationToken cancellationToken);
    }
}
