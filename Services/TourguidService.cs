using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

public class TourguidService(TourismContext Db) : ITourguidService
{
    private readonly TourismContext db = Db;

    public async Task<Result<TourguidProfile>> Profile(string id , CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users
            .Include(i => i.InverseTourguid).Include(i => i.TourguidAndPlaces)
            .SingleOrDefaultAsync(i => i.Id == id);
        if (tourguid is null)
            return Result.Failure<TourguidProfile>(TourguidErrors.TourguidNotFound);
        var result = tourguid.Adapt<TourguidProfile>();
        result.tourists = tourguid.InverseTourguid.Adapt<List<Tourist>>();
        result.TouristsCount = result.tourists.Count();
        result.placeName = tourguid.TourguidAndPlaces.Select(i => i.PlaceName).First();
        return Result.Success(result);
    }
    public async Task<Result> UpdateProfile(string id, TourguidUpdateProfile request, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.FindAsync(id);
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        tourguid = request.Adapt(tourguid);
        db.Users.Update(tourguid);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> RemoveTourist (string tourguidid , string Touristid , CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == Touristid && i.TourguidId == tourguidid);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        user.TourguidId = null;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
