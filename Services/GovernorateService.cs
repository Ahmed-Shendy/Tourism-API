using Mapster;
using Microsoft.EntityFrameworkCore;
using Tourism_Api.Entity.Governorate;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services
{
    public class GovernorateService(TourismContext Db) : IGovernorateService
    {
        private readonly TourismContext _Db = Db;

        public async Task<Result<IEnumerable<GovernorateResponse>>> GetGovernorate(CancellationToken cancellationToken)
        {
            var result = await _Db.Governorates.ToListAsync(cancellationToken);
            var resonse = result.Adapt<IEnumerable<GovernorateResponse>>();
            return Result.Success(resonse);
        }

        public async Task<Result<GovernorateAndPLacesResponse>> GetGovernorateAndPlacesAsync(string Name, CancellationToken cancellationToken)
        {
            var result = await _Db.Governorates.Include(x => x.Places).FirstOrDefaultAsync(x => x.Name == Name);
               
            if (result == null)
                return Result.Failure<GovernorateAndPLacesResponse>(GovernerateErrors.EmptyGovernerate);
            var response = result.Adapt<GovernorateAndPLacesResponse>();
            return Result.Success(response);

        }
    }
}
