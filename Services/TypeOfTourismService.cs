using Mapster;
using Microsoft.EntityFrameworkCore;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.TypeOfTourism;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Pagnations;
using Tourism_Api.Services.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tourism_Api.Services
{
    public class TypeOfTourismService(TourismContext dbcontext) : ITypeOfTourismService
    {
        private readonly TourismContext _dbcontext = dbcontext;



        public async Task<Result<TypeOfTourismAndPlacesResponse>> GetTypeOfTourismAndPlaces(String Name, CancellationToken cancellationToken)
        {
            var type = _dbcontext.Type_of_Tourism_Places
                .Include(x => x.Place)
                .Where(x => x.Tourism_Name == Name);
            if (!type.Any())
             return Result.Failure<TypeOfTourismAndPlacesResponse>(TypeOfTourismErrors.NotFound);

            var places = type.Select(i => i.Place.Adapt<ALLPlaces>()).ToList();
            var result = new TypeOfTourismAndPlacesResponse();
            result.ALLPlaces = places;
            result.Tourism_Name = Name;
            return Result.Success(result); 
        }


        public async Task<Result<PaginatedList<TypeOfTourismALLPlaces>>> GetTypeOfTourismAndPlacesAsync
            (RequestFiltersScpical requestFilters, CancellationToken cancellationToken)
        {
            var typesAndPlaces = _dbcontext.Type_of_Tourism_Places
                .Include(x => x.Place)
                .Where(x => x.Tourism_Name == requestFilters.Name);
            if (typesAndPlaces == null)
                return Result.Failure<PaginatedList<TypeOfTourismALLPlaces>>(TypeOfTourismErrors.EmptyTypeOfTourism);
           
            //result.PlaceNames.Tourism_Name = Name;
            IQueryable<TypeOfTourismALLPlaces> Query = typesAndPlaces
                .Select(x => new TypeOfTourismALLPlaces
                {
                    Name = x.Place.Name,
                    Photo = x.Place.Photo!,
                    GoogleRate = x.Place.GoogleRate,
                    Tourism_Name = x.Tourism_Name
                });

            if (!string.IsNullOrWhiteSpace(requestFilters.SearchValue))
            {
                Query = Query.Where(i => i.Name.Contains(requestFilters.SearchValue));
            }

            // use this to sort data by column or Rate
            Query = Query.OrderByDescending(i => i.GoogleRate);

            var Responce = await PaginatedList<TypeOfTourismALLPlaces>.CreateAsync(Query, requestFilters.PageNumber, requestFilters.PageSize);
            return Result.Success(Responce);
        }

        public async Task<Result<IEnumerable<TypeOfTourismResponse>>> GetAllTypeOfTourismAsync(CancellationToken cancellationToken)
        {
            var typeOfTourism = await _dbcontext.TypeOfTourisms.ToListAsync();

            var result = typeOfTourism.Adapt<IEnumerable<TypeOfTourismResponse>>();

            return Result.Success(result);
        }

        public async Task<Result<List<string>>> AllTypeOfTourismName(CancellationToken cancellationToken)
        {
            var result = await _dbcontext.TypeOfTourisms.Select(x => x.Name).ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
