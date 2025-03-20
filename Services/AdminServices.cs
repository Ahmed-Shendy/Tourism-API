using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tourism_Api.Entity.Places;
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
        var place = request.Adapt<Place>();
        await db.Places.AddAsync(place, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success(place.Adapt<PlacesDetails>());
    }

    public async Task<Result> DeletePlace(string name, CancellationToken cancellationToken = default)
    {
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == name);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
        db.Places.Remove(place);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> UpdatePlace(AddPlaceRequest request, CancellationToken cancellationToken = default)
    {
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == request.Name);
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
        var save = await UserMander.CreateAsync(tourguid, request.Password);
        if (save.Succeeded)
        {
            await UserMander.AddToRoleAsync(tourguid, DefaultRoles.Tourguid);
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
         var ChecktourguidPlace = await db.TourguidAndPlaces.SingleOrDefaultAsync(i => i.PlaceName == placeName && i.TouguidId == tourguidId);
        if (ChecktourguidPlace is not null)
            return Result.Failure(TourguidErrors.TourguidPlaces);
        var tourguidPlace = new TourguidAndPlaces
        {
            PlaceName = place.Name,
            TouguidId = tourguid.Id
        };
        await db.TourguidAndPlaces.AddAsync(tourguidPlace, cancellationToken);
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

    public async Task<Result<List<AllTourguids>>> DisplayAllTourguid(CancellationToken cancellationToken = default)
    {
        var result = await db.Users.Include(i => i.TourguidAndPlaces).ThenInclude(i => i.Place)
            .Where(i => i.Role == "Tourguid")
            .Select(i => new AllTourguids 
            { Id = i.Id, Email = i.Email, Name = i.Name, Phone = i.Phone, Photo = i.Photo ?? "",
                PlaceNames = i.TourguidAndPlaces.Select(i => i.PlaceName).ToList() ,
                PlaceCount = i.TourguidAndPlaces.Count,
               countOfTourisms = db.Users.Where(UserTourguid => UserTourguid.TourguidId == i.Id).Count()
            }).OrderByDescending(i => i.countOfTourisms).ToListAsync(cancellationToken);
        
       // var result = tourguids.Adapt<List<AddTourguidRequest>>();

        return Result.Success(result);
    }

}
