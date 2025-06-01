using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using System.Linq;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.user;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Pagnations;
using Tourism_Api.Services.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tourism_Api.Services;
public class PlaceService(TourismContext Db , HybridCache cache) : IPlaceService
{
    private readonly TourismContext db = Db;
    private readonly HybridCache cache = cache;

   
    public async Task<Result<All_Places>> DisplayAllPlaces(CancellationToken cancellationToken = default)
    {
        string cacheKey = $"AllPlaces";

        
        var Places = await cache.GetOrCreateAsync<All_Places>(cacheKey, async cacheEntery =>
        {
            All_Places all_Places = new All_Places();
            all_Places.Places =  await db.Places
                .Select(i => new ALLPlaces { Name = i.Name, Photo = i.Photo ?? "" , GoogleRate = i.GoogleRate })
                .OrderByDescending(i => i.GoogleRate)
                .ToListAsync(cancellationToken);

             return all_Places;
        });

        // use this to remove the cache
        //await cache.RemoveAsync($"AllPlaces");


        return Places is not null && Places.Places.Any()
            ? Result.Success(Places)
            : Result.Failure<All_Places>(PlacesErrors.PlacesNotFound);
    }

    public async Task<PaginatedList<ALLPlaces>> DisplayAllPlacesByPagnation(RequestFilters requestFilters , CancellationToken cancellationToken = default)
    {
        IQueryable<ALLPlaces> query = db.Places.ProjectToType<ALLPlaces>();
                
        if (!string.IsNullOrWhiteSpace(requestFilters.SearchValue))
        {
            query = query.Where(i => i.Name!.Contains(requestFilters.SearchValue));
        }

        // use this to sort data by column or Rate
        query = query.OrderByDescending(i => i.GoogleRate);

        var result = await PaginatedList<ALLPlaces>.CreateAsync(query, requestFilters.PageNumber, requestFilters.PageSize);
        return result!;

    }
    public async Task<Result<PlacesDetails>> PlacesDetails(string userid , string name  ,CancellationToken cancellationToken = default)
    {
       // string name = "Ahmed%20Ayman";
        name = name.Replace("%20", " ");

        var result = await db.Places
            .Include(i => i.PlaceRates).ThenInclude(i => i.User)
            .Include(i => i.GovernmentNameNavigation)
            .Include(i => i.FavoritePlaces)
            .Include(i => i.Comments).ThenInclude(i => i.User).Include(i => i.Type_Of_Tourism_Places)    
            .SingleOrDefaultAsync(i => i.Name == name, cancellationToken);

        if (result is null)
            return Result.Failure<PlacesDetails>(PlacesErrors.PlacesNotFound);

        var place = result.Adapt<PlacesDetails>();
        place.comments = result!.Comments.Select(i => new UserComment
        {
            Content = i.Content,
            UserName = i.User.Name,
            photo = i.User.Phone,
            UserId = i.UserId,
            id = i.Id,
        }
        ).ToList();

       var AllTourguid =  await db.TourguidAndPlaces
            .Include(i => i.Touguid)
            .ThenInclude(i => i.Tourguid_Rates)
            .Where(i => i.PlaceName == result.Name && i.Touguid.EmailConfirmed)
            .ToListAsync(cancellationToken);
        // place.Tourguids = AllTourguid.Adapt<List<Tourguids>>();

        place.IsFavorite = result.FavoritePlaces.Any(i => i.UserId == userid);
        place.UserRates = result.PlaceRates.Select(i => new UswerRate
        {
            UserId = i.UserId,
            UserName = i.User.Name,
            photo = i.User.Photo ?? "",
            Rate = i.Rate,
        }).ToList();
        place.Tourguids = AllTourguid.Select(i => new Tourguids
        {
            Id = i.Touguid.Id,
            Name = i.Touguid.Name,
            Email = i.Touguid.Email,    
            Phone = i.Touguid.Phone,
            Gender = i.Touguid.Gender,
            Photo = i.Touguid.Photo ?? "",
            IsActive = i.Touguid.IsActive,
            rate = db.Tourguid_Rates
            .Where(j => j.tourguidId == i.Touguid.Id)
            .Select(j => j.rate).Count() == 0 ? 0 : Math.Round((decimal)db.Tourguid_Rates
            .Where(j => j.tourguidId == i.Touguid.Id)
            .Select(j => j.rate).Average(), 1),
           // rate = (decimal?)i.Touguid.Tourguid_Rates.Where(x => x.tourguidId == i.Touguid.Id).Average(x => x.rate),
        }).ToList();
        place.TypeOfTourism = result.Type_Of_Tourism_Places.Select(i => i.Tourism_Name).ToList();

        //place.Tourguids = result.Tourguids!.Select(i => new Tourguids
        //{
        //    Id = i.Id,
        //    Name = i.Name,
        //    Email = i.Email,
        //    Phone = i.Phone,
        //    Photo = i.Photo ?? "",
        //    Gender = i.Gender,
        //}).ToList();

        return Result.Success(place);
    }

    public async Task<Result<List<string>>> AllPlacesName(CancellationToken cancellationToken)
    {
        var result = await db.Places.Select(x => x.Name).ToListAsync(cancellationToken);

        return Result.Success(result);
    }
    // search for place by name
    public async Task<Result<List<ALLPlaces>>> SearchForPlace(string name, CancellationToken cancellationToken = default)
    {
        var result = await db.Places.Where(i => i.Name.Contains(name)).ToListAsync(cancellationToken);
        return Result.Success(result.Adapt<List<ALLPlaces>>());
    }

}
