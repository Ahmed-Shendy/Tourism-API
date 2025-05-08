using Tourism_Api.Entity.Governorate;
using Tourism_Api.Pagnations;

namespace Tourism_Api.Services.IServices
{
    public interface IGovernorateService
    {
        Task<Result<PaginatedList<GovernorateResponse>>> GetGovernoratePagnation(RequestFilters requestFilters,  CancellationToken cancellationToken);
        Task<Result<GovernorateAndPLacesResponse>> GetGovernorateAndPlace(string Name, CancellationToken cancellationToken);
        Task<Result<PaginatedList<GovernorateALLPlaces>>> GetGovernorateAndPlacesAsync
            (RequestFiltersScpical requestFilters, CancellationToken cancellationToken);
        Task<Result<List<string>>> GetGovernoratesName(CancellationToken cancellationToken);
        Task<Result<Governorates>> GetGovernorate(CancellationToken cancellationToken);
    }
}
