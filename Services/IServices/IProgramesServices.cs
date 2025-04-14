using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Programs;

namespace Tourism_Api.Services.IServices;

public interface IProgramesServices
{

    Task<Result<List<ALLPlaces>>> RecomendPlaces(string userId , CancellationToken cancellationToken = default);
    Task<Result<TripDetails>> TripDetails(string userId, string TripName, CancellationToken cancellationToken = default);
    Task<Result<List<TripsResponse>>> AllTripsInProgram(string userid, CancellationToken cancellationToken = default);

    Task<Result> GetProgram(string userId, string programName, CancellationToken cancellationToken = default);
    Task<Result<List<string>>> Tourism_Type(CancellationToken cancellationToken = default);
    Task<Result<List<string>>> With_Family(CancellationToken cancellationToken = default);
    Task<Result<List<string>>> Accommodation_Type(CancellationToken cancellationToken = default);
    Task<Result<List<string>>> Preferred_Destination(CancellationToken cancellationToken = default);
    Task<Result<List<string>>> Travel_Purpose(CancellationToken cancellationToken = default);

}
