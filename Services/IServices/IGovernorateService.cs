using Tourism_Api.Entity.Governorate;

namespace Tourism_Api.Services.IServices
{
    public interface IGovernorateService
    {
        Task<Result<IEnumerable<GovernorateResponse>>> GetGovernorate(CancellationToken cancellationToken);
        Task<Result<GovernorateAndPLacesResponse>> GetGovernorateAndPlacesAsync(string Name, CancellationToken cancellationToken);
        Task<Result<List<string>>> GetGovernoratesName(CancellationToken cancellationToken);
    }
}
