using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Programs;

namespace Tourism_Api.Services.IServices;

public interface IProgramesServices
{

    Task<Result<List<ALLPlaces>>> RecomendPlaces(string userId , CancellationToken cancellationToken = default);
    Task<Result<ProgramDetails>> ProgramDetails(string userId, CancellationToken cancellationToken = default);
}
