using Tourism_Api.Entity.Governorate;
using Tourism_Api.Pagnations;

namespace Tourism_Api.Services.IServices
{
    public interface IGovernorateService
    {
        Task<Result<PaginatedList<GovernorateResponse>>> GetGovernorate(RequestFilters requestFilters,  CancellationToken cancellationToken);
        Task<Result<GovernorateAndPLacesResponse>> GetGovernorateAndPlacesAsync(string Name, CancellationToken cancellationToken);
        Task<Result<List<string>>> GetGovernoratesName(CancellationToken cancellationToken);
    }
}
