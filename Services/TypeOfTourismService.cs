using Mapster;
using Microsoft.EntityFrameworkCore;
using Tourism_Api.Entity.TypeOfTourism;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services
{
    public class TypeOfTourismService(TourismContext dbcontext) : ITypeOfTourismService
    {
        private readonly TourismContext _dbcontext = dbcontext;

        public async Task<Result<IEnumerable<TypeOfTourismAndPlacesResponse>>> GetTypeOfTourismAndPlacesAsync(String Name ,CancellationToken cancellationToken)
        {
            var typesAndPlaces = await _dbcontext.TypeOfTourisms.Include(x => x.PlaceNames).Where(x=>x.Name == Name).ToListAsync();
            if (typesAndPlaces == null)
                return Result.Failure<IEnumerable<TypeOfTourismAndPlacesResponse>>(TypeOfTourismErrors.EmptyTypeOfTourism);
            var result = typesAndPlaces.Adapt<IEnumerable<TypeOfTourismAndPlacesResponse>>();
            return Result.Success(result);
        }

        public async Task<Result<IEnumerable<TypeOfTourismResponse>>> GetAllTypeOfTourismAsync(CancellationToken cancellationToken)
        {
            var typeOfTourism = await _dbcontext.TypeOfTourisms.ToListAsync();

            var result = typeOfTourism.Adapt<IEnumerable<TypeOfTourismResponse>>();

            return Result.Success(result);
        }
    }
}
