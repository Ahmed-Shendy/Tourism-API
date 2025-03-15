using Tourism_Api.Entity.TypeOfTourism;

namespace Tourism_Api.Services.IServices
{
    public interface ITypeOfTourismService
    {
        Task<Result<IEnumerable<TypeOfTourismResponse>>> GetAllTypeOfTourismAsync(CancellationToken cancellationToken);
        Task<Result<IEnumerable<TypeOfTourismAndPlacesResponse>>> GetTypeOfTourismAndPlacesAsync(String Name , CancellationToken cancellationToken);

    }
}
