using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Tourism_Api.Entity.Governorate;
using Tourism_Api.Entity.Places;
using Tourism_Api.model.Context;
using Tourism_Api.Pagnations;
using Tourism_Api.Services.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tourism_Api.Services
{
    public class GovernorateService(TourismContext Db) : IGovernorateService
    {
        private readonly TourismContext _Db = Db;

        public async Task<Result< PaginatedList<GovernorateResponse> >> GetGovernorate(RequestFilters requestFilters ,CancellationToken cancellationToken)
        {
            var query = _Db.Governorates.Select(i => new GovernorateResponse
            {
                Name = i.Name,
                Photo = i.Photo,

            });
            if (!string.IsNullOrWhiteSpace(requestFilters.SearchValue))
            {
                query = query.Where(i => i.Name!.Contains(requestFilters.SearchValue));
            }
            // var resonse = result.Adapt<IEnumerable<GovernorateResponse>>();

            var result = await PaginatedList<GovernorateResponse>.CreateAsync(query, requestFilters.PageNumber, requestFilters.PageSize);

            return Result.Success(result);
        }

        public async Task<Result<GovernorateAndPLacesResponse>> GetGovernorateAndPlacesAsync(string Name, CancellationToken cancellationToken)
        {
            var result = await _Db.Governorates.Include(x => x.Places).FirstOrDefaultAsync(x => x.Name == Name);
               
            if (result == null)
                return Result.Failure<GovernorateAndPLacesResponse>(GovernerateErrors.EmptyGovernerate);
            var response = result.Adapt<GovernorateAndPLacesResponse>();
            return Result.Success(response);

        }

        public async Task<Result<List<string>>> GetGovernoratesName( CancellationToken cancellationToken)
        {
            var result = await _Db.Governorates.Select(x => x.Name).ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
