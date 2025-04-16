using Mapster;
using Microsoft.EntityFrameworkCore;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.TypeOfTourism;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services
{
    public class TypeOfTourismService(TourismContext dbcontext) : ITypeOfTourismService
    {
        private readonly TourismContext _dbcontext = dbcontext;

        public async Task<Result<TypeOfTourismAndPlacesResponse>> GetTypeOfTourismAndPlacesAsync(String Name ,CancellationToken cancellationToken)
        {
            var typesAndPlaces = await _dbcontext.Type_of_Tourism_Places
                .Include(x => x.Place)
                .Where(x=>x.Tourism_Name == Name).ToListAsync();
            if (typesAndPlaces == null)
                return Result.Failure<TypeOfTourismAndPlacesResponse>(TypeOfTourismErrors.EmptyTypeOfTourism);
            var result = new TypeOfTourismAndPlacesResponse();
            result.Tourism_Name = Name;
            result.PlaceNames = typesAndPlaces.Select(x => x.Place.Adapt<ALLPlaces>()).ToList();
            
            return Result.Success(result);
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
