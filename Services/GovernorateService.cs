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

        public async Task<Result<PaginatedList<GovernorateResponse> >> GetGovernoratePagnation(RequestFilters requestFilters ,CancellationToken cancellationToken)
        {
            var query = _Db.Governorates.Select(i => new GovernorateResponse
            {
                Name = i.Name,
                Photo = i.Photo,

            });
            if (!string.IsNullOrWhiteSpace(requestFilters.SearchValue))
            {
                query = query.Where(i => i.Name.Contains(requestFilters.SearchValue));
            }
            // var resonse = result.Adapt<IEnumerable<GovernorateResponse>>();

            var result = await PaginatedList<GovernorateResponse>.CreateAsync(query, requestFilters.PageNumber, requestFilters.PageSize);

            return Result.Success(result);
        }
        public async Task<Result<List <GovernorateResponse>>> GetGovernorate( CancellationToken cancellationToken)
        {
            var result = await _Db.Governorates.
                Select(i => new GovernorateResponse { Name = i.Name, Photo = i.Photo, }).ToListAsync(cancellationToken);
            
            return Result.Success(result);
        }


        public async Task<Result<GovernorateAndPLacesResponse>> GetGovernorateAndPlace(string Name, CancellationToken cancellationToken)
        {
            Name = Name.Replace("%20", " ");
            var result = await _Db.Governorates.Include(x => x.Places).FirstOrDefaultAsync(x => x.Name == Name);

            if (result == null)
                return Result.Failure<GovernorateAndPLacesResponse>(GovernerateErrors.EmptyGovernerate);
            var response = result.Adapt<GovernorateAndPLacesResponse>();
            return Result.Success(response);

        }
        public async Task<Result<PaginatedList<GovernorateALLPlaces>>> GetGovernorateAndPlacesAsync
            (RequestFiltersScpical requestFilters, CancellationToken cancellationToken)
        {
           
            
            var Gavernment = await _Db.Governorates.SingleOrDefaultAsync(x => x.Name == requestFilters.Name, cancellationToken);
            if (Gavernment == null)
                return Result.Failure<PaginatedList<GovernorateALLPlaces>>(GovernerateErrors.EmptyGovernerate);

            IQueryable<GovernorateALLPlaces> Query = _Db.Places.Where( x => x.GovernmentName == requestFilters.Name)
                .Select(i => new GovernorateALLPlaces
            {
                Governorate_Name = Gavernment.Name,
                GoogleRate = i.GoogleRate,
                Name = i.Name,
                Photo = i.Photo
            });


            if (!string.IsNullOrWhiteSpace(requestFilters.SearchValue))
            {
                Query = Query.Where(i => i.Name.Contains(requestFilters.SearchValue));
            }

            // use this to sort data by column or Rate
            Query = Query.OrderByDescending(i => i.GoogleRate);

            var Responce = await PaginatedList<GovernorateALLPlaces>.CreateAsync(Query, requestFilters.PageNumber, requestFilters.PageSize);
            return Result.Success(Responce);

        }

        public async Task<Result<List<string>>> GetGovernoratesName( CancellationToken cancellationToken)
        {
            var result = await _Db.Governorates.Select(x => x.Name).ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
