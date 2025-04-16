using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tourism_Api.Entity.Admin;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Programs;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.user;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

   
public class AdminServices(TourismContext db, UserManager<User> user) : IAdminServices
{
    private readonly TourismContext db = db;
    private readonly UserManager<User> UserMander = user;

    public async Task<Result<PlacesDetails>> AddPlace(AddPlaceRequest request, CancellationToken cancellationToken = default)
    {
        var placeIsExists = await db.Places.AnyAsync(x => x.Name == request.Name, cancellationToken);
        if (placeIsExists)
            return Result.Failure<PlacesDetails>(PlacesErrors.PlacesUnque);
        
        var GavernmentIsExists = await db.Governorates.AnyAsync(x => x.Name == request.GovernmentName, cancellationToken);
        if (!GavernmentIsExists)
            return Result.Failure<PlacesDetails>(GovernerateErrors.EmptyGovernerate);
        //var TypeOfTourism = new Typeof
        var place = request.Adapt<Place>();
        await db.Places.AddAsync(place, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        foreach (var item in request.TypeOfTourism)
        {
            var TypeOfTourismPlace = new Type_of_Tourism_Places
            {
                Place_Name = place.Name,
                Tourism_Name = item
            };
            await db.Type_of_Tourism_Places.AddAsync(TypeOfTourismPlace, cancellationToken);
        }
       
        await db.SaveChangesAsync(cancellationToken);
        var result = place.Adapt<PlacesDetails>();
        result.TypeOfTourism = request.TypeOfTourism;
        return Result.Success(result);
    }

    public async Task<Result> DeletePlace(string name, CancellationToken cancellationToken = default)
    {
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == name);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
        db.Places.Remove(place);
        db.Type_of_Tourism_Places.RemoveRange(db.Type_of_Tourism_Places.Where(i => i.Place_Name == name));
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> UpdatePlace(string PlaceName , UpdatePlaceRequest request, CancellationToken cancellationToken = default)
    {
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == PlaceName);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
        place = request.Adapt(place);
        db.Places.Update(place);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> AddTourguid(AddTourguidRequest request, CancellationToken cancellationToken = default)
    {
        var tourguid = request.Adapt<User>();
        tourguid.Role = "Tourguid";
        tourguid.UserName = tourguid.Email;
        var emailIsExists = await db.Users.AnyAsync(x => x.Email == tourguid.Email, cancellationToken);
        if (emailIsExists)
            return Result.Failure<UserRespones>(UserErrors.EmailUnque);
        if (request.PlaceName != null)
        {
            var place = await db.Places.SingleOrDefaultAsync(i => i.Name == request.PlaceName);
            if (place is null)
                return Result.Failure(PlacesErrors.PlacesNotFound);
        }
        if (request.TripName != null)
        {
            var trip = await db.Trips.SingleOrDefaultAsync(i => i.Name == request.TripName);
            if (trip is null)
                return Result.Failure(ProgramErorr.ProgramNotFound);
        }
        var save = await UserMander.CreateAsync(tourguid, request.Password);
        if (save.Succeeded)
        {
            await UserMander.AddToRoleAsync(tourguid, DefaultRoles.Tourguid);

            if (request.PlaceName != null && request.TripName == null)
            {

                var tourguidPlace = new TourguidAndPlaces
                {
                    TouguidId = tourguid.Id,
                    PlaceName = request.PlaceName
                };
                await db.TourguidAndPlaces.AddAsync(tourguidPlace, cancellationToken);
            }
            else if (request.TripName != null)
            {
                tourguid.TripName = request.TripName;
            }

            await db.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
        //await UserMander.AddToRoleAsync(tourguid, DefaultRoles.tourguid);
        //await db.Users.AddAsync(tourguid, cancellationToken);
        //await db.SaveChangesAsync(cancellationToken);
        return Result.Failure(UserErrors.notsaved);
    }
    public async Task<Result> DeleteTourguid(string id, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == id && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        
        db.Users.Remove(tourguid);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
   
    public async Task<Result> AddTourguidPlace(string tourguidId, string placeName, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == tourguidId && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == placeName);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);


        // use it if you want to check if the place is already added to the tourguid
        //var ChecktourguidPlace = await db.TourguidAndPlaces.SingleOrDefaultAsync(i => i.PlaceName == placeName && i.TouguidId == tourguidId);
        //if (ChecktourguidPlace is not null)
        //{

        //    return Result.Failure(TourguidErrors.TourguidPlaces);
        //}
        //var tourguidPlace = new TourguidAndPlaces
        //{
        //    PlaceName = place.Name,
        //    TouguidId = tourguid.Id
        //};
        //await db.TourguidAndPlaces.AddAsync(tourguidPlace, cancellationToken);
        //await db.SaveChangesAsync(cancellationToken);
        //return Result.Success();


       // use it if you want to the tourguid added one of the place
        var ChecktourguidPlace = await db.TourguidAndPlaces.SingleOrDefaultAsync(i => i.TouguidId == tourguidId);
        if (ChecktourguidPlace is not null)
        {
            //db.TourguidAndPlaces.Remove(ChecktourguidPlace);
            //await db.SaveChangesAsync(cancellationToken);

            var cleanTourguid = await db.Users
                .Where(i => i.TourguidId == tourguidId).ToListAsync(cancellationToken);
            if (cleanTourguid.Any())
            {
                foreach (var user in cleanTourguid)
                {
                    user.TourguidId = null;
                }
                await db.SaveChangesAsync(cancellationToken);
            }
            ChecktourguidPlace.PlaceName = placeName;
            ChecktourguidPlace.MoveToPlace = null;
        }
        else
        {
            var tourguidPlace = new TourguidAndPlaces
            {
                PlaceName = place.Name,
                TouguidId = tourguid.Id
            };
            await db.TourguidAndPlaces.AddAsync(tourguidPlace, cancellationToken);
        }
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> DeleteTourguidPlace(string tourguidId, string placeName, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == tourguidId && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(UserErrors.UserNotFound);
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == placeName);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
        var tourguidPlace = await db.TourguidAndPlaces.SingleOrDefaultAsync(i => i.PlaceName == placeName && i.TouguidId == tourguidId);
        if (tourguidPlace is null)
            return Result.Failure(UserErrors.UserNotFound);
        db.TourguidAndPlaces.Remove(tourguidPlace);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<Dashboard>> DisplayAll(CancellationToken cancellationToken = default)
    {
        Dashboard DachshundResult = new Dashboard();
        DachshundResult.allTourguids = await db.Users.Include(i => i.TourguidAndPlaces).ThenInclude(i => i.Place)
           .Where(i => i.Role == "Tourguid").OrderByDescending(i => i.Score)
           .Select(i => new AllTourguids
           {
               Id = i.Id,
               Email = i.Email,
               Name = i.Name,
               Phone = i.Phone,
               PlaceNames = i.TourguidAndPlaces.Select(i => i.PlaceName).ToList(),
               Age = i.Age,
               Gender = i.Gender,
               PlaceCount = i.TourguidAndPlaces.Count,
               countOfTourisms = i.Score
               //countOfTourisms = db.Users.Where(UserTourguid => UserTourguid.TourguidId == i.Id).Count()
           }).Take(10).ToListAsync(cancellationToken);

        DachshundResult.CountFamle =  db.Users.Where(i => i.Gender == "Female").Count();
        DachshundResult.CountMale = db.Users.Where(i => i.Gender == "Male").Count();
        DachshundResult.peopleForCountries = await db.Users.Where(List => List.Role == "User")
                    .GroupBy(u => u.Country)
                    .Select(g => new PeopleForCountry
                    {
                        country = g.Key!,
                        count = g.Count()
                    }).OrderByDescending(i => i.count).ToListAsync(cancellationToken);

        // var result = tourguids.Adapt<List<AddTourguidRequest>>();

         return Result.Success(DachshundResult);
    }

    public async Task<Result<TransferRequests>> TransferRequest( CancellationToken cancellationToken = default)
    {
        
        var transferRequests = db.TourguidAndPlaces.Include(i => i.Touguid)
            .Where(i => i.MoveToPlace != null).Select ( i => new TourguidTransferRequest
            {
                TourguidId = i.TouguidId,
                TourguidPhoto = i.Touguid.Photo!,
                TourguidName = i.Touguid.Name,
                PlaceName = i.Place.Name,
                MovedPlace = i.MoveToPlace!,
            }).ToList();
        var result = new TransferRequests
        {
            tourguidTransferRequests = transferRequests,
            Count = transferRequests.Count
        };
        return Result.Success(result);
    }
 
    public async Task<Result> TransferRequestDecline(string tourguidId , CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == tourguidId && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(UserErrors.UserNotFound);
       
        var tourguidPlace = await db.TourguidAndPlaces.SingleOrDefaultAsync(i => i.TouguidId == tourguidId);
        if (tourguidPlace is null)
            return Result.Failure(UserErrors.UserNotFound);
        tourguidPlace.MoveToPlace = null;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<List<string>>> DisplayAllPrograms(CancellationToken cancellationToken = default)
    {
        var programs = await db.Programs.Select(i => i.Name).ToListAsync(cancellationToken);
        if (programs is null)
            return Result.Failure<List<string>>(ProgramErorr.ProgramNotFound);
        return Result.Success(programs);
    }

    public async Task<Result> AddTourguidTrip(TourguidTrips request , CancellationToken cancellationToken = default)
    {
        var  user = await db.Users.SingleOrDefaultAsync(i => i.Id == request.TourguidId && i.Role == "Tourguid");
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var trip = await db.Trips.SingleOrDefaultAsync(i => i.Name == request.TripName);
        if (trip is null)
            return Result.Failure(ProgramErorr.ProgramNotFound);
        user.TripName = request.TripName;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

}
