using Tourism_Api.Entity.TypeOfTourism;

namespace Tourism_Api.Services.IServices
{
    public interface ITypeOfTourismService
    {
        Task<Result<IEnumerable<TypeOfTourismResponse>>> GetAllTypeOfTourismAsync(CancellationToken cancellationToken);
        Task<Result<TypeOfTourismAndPlacesResponse>> GetTypeOfTourismAndPlacesAsync(String Name , CancellationToken cancellationToken);
        Task<Result<List<string>>> AllTypeOfTourismName(CancellationToken cancellationToken);
    }
}
