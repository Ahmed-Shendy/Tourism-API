using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.upload;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;
using static System.Net.Mime.MediaTypeNames;

namespace Tourism_Api.Services;

public class TourguidService(IWebHostEnvironment webHostEnvironment , TourismContext Db) : ITourguidService
{
    private readonly TourismContext db = Db;

    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";

    public async Task<Result<TourguidProfile>> Profile(string id , CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users
            .Include(i => i.InverseTourguid).Include(i => i.TourguidAndPlaces).ThenInclude(i => i.Place)
            .Include(i => i.Program).ThenInclude(p => p!.PlaceNames)
            .SingleOrDefaultAsync(i => i.Id == id);
        if (tourguid is null)
            return Result.Failure<TourguidProfile>(TourguidErrors.TourguidNotFound);
        var result = tourguid.Adapt<TourguidProfile>();
        result.tourists = tourguid.InverseTourguid.Adapt<List<Tourist>>();
        result.TouristsCount = result.tourists.Count();
       if (tourguid.ProgramName == null)
            result.places = tourguid.TourguidAndPlaces.Select(i => i.Place).Adapt<List<placesinfo>>().ToList();
        // result.placeName[0] = tourguid.TourguidAndPlaces.Select(i => i.PlaceName).First();
        else
            result.places = tourguid.Program!.PlaceNames.Adapt<List<placesinfo>>().ToList();
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

    public async Task<Result<TourguidPublicProfile>> PublicProfile(string id, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users
            .Include(i => i.InverseTourguid).Include(i => i.TourguidAndPlaces).ThenInclude(i => i.Place)
            .Include(i => i.Program).ThenInclude(p => p!.PlaceNames)
            .SingleOrDefaultAsync(i => i.Id == id);
        if (tourguid is null)
            return Result.Failure<TourguidPublicProfile>(TourguidErrors.TourguidNotFound);
        var result = tourguid.Adapt<TourguidPublicProfile>();
        result.tourists = tourguid.InverseTourguid.Adapt<List<Tourist>>();
        result.TouristsCount = result.tourists.Count();
        if (tourguid.ProgramName == null)
            result.places = tourguid.TourguidAndPlaces.Select(i => i.Place).Adapt<List<placesinfo>>().ToList();
        // result.placeName[0] = tourguid.TourguidAndPlaces.Select(i => i.PlaceName).First();
        else
            result.places = tourguid.Program!.PlaceNames.Adapt<List<placesinfo>>().ToList();
        return Result.Success(result);
    }

    public async Task<Result> UploadPhoto(string id, IFormFile image, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.FindAsync(id);
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);

        //var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
        //using (var stream = new FileStream(path, FileMode.Create))
        //{
        //    await image.CopyToAsync(stream, cancellationToken);
        //}
        var path = Path.Combine(_imagesPath, image.FileName);
        if (tourguid.Photo == null)
        {
            using var stream = File.Create(path);
            await image.CopyToAsync(stream, cancellationToken);
        }
        else
        {
            var oldPath = Path.Combine(_imagesPath, tourguid.Photo);
            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }
            using var stream = File.Create(path);
            await image.CopyToAsync(stream, cancellationToken);
        }
        tourguid.Photo = image.FileName;
        tourguid.ContentType = image.ContentType;
        db.Users.Update(tourguid);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeletePhoto(string id, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.FindAsync(id);
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        var path = Path.Combine(_imagesPath, tourguid.Photo!);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        tourguid.Photo = null;
        db.Users.Update(tourguid);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> MoveToPlaces(string tourguidid, string placeName, CancellationToken cancellationToken = default)
    {
        var tourguidPlace = await db.TourguidAndPlaces
           .SingleOrDefaultAsync(i => i.TouguidId == tourguidid);
        if (tourguidPlace is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == placeName);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
      
        tourguidPlace!.MoveToPlace = placeName;

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<(byte[] fileContent , string ContentType, string fileName)> DownloadAsync(string id, CancellationToken cancellationToken = default)
    {
        var file = await db.Users.FindAsync(id);

        if (file is null)
            return ([], string.Empty , string.Empty);

        var path = Path.Combine(_imagesPath, file.Photo!);

        MemoryStream memoryStream = new();
        using FileStream fileStream = new(path, FileMode.Open);
        fileStream.CopyTo(memoryStream);

        memoryStream.Position = 0;

        return (memoryStream.ToArray() , file.ContentType! , file.Photo!);
    }

}
