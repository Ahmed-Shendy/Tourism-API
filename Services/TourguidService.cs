using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq;
using Tourism_Api.Entity.Programs;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.upload;
using Tourism_Api.Entity.user;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tourism_Api.Services;

public class TourguidService(IWebHostEnvironment webHostEnvironment
    , UserManager<User> user
    , TourismContext Db) : ITourguidService
{
    private readonly UserManager<User> UserMander = user;
    private readonly TourismContext db = Db;

    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";
    private readonly string _filesPath = $"{webHostEnvironment.WebRootPath}/files";

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
       // result.TouristsCount = result.tourists.Count();
        var rate = db.Tourguid_Rates
           .Where(i => i.tourguidId == id)
           .Select(i => i.rate);
        result.Rate = rate.Count() == 0 ? 0 : Math.Round((decimal)rate.Average(), 1);
        result.AllLangues = await db.Langues
            .Where(i => i.TourguidId == id).AsNoTracking()
            .Select(i => i.Langue).ToListAsync(cancellationToken);

        result.RateGroup = db.Tourguid_Rates
        .Where(i => i.tourguidId == id).GroupBy(i => i.rate)
        .Select(i => new RateGroup
        {
            value = i.Key,
            count = i.Count()
        }).ToList();

        if (tourguid.TripName == null)
            result.places = tourguid.TourguidAndPlaces.Select(i => i.Place).Adapt<List<placesinfo>>().ToList();
        // result.placeName[0] = tourguid.TourguidAndPlaces.Select(i => i.PlaceName).First();
        else
            result.TripName = tourguid.TripName;
        //result.places = tourguid.Program!.PlaceNames.Adapt<List<placesinfo>>().ToList();
        return Result.Success(result);
    }
    public async Task<Result> UpdateProfile(string id, TourguidUpdateProfile request, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.FindAsync(id);
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        tourguid = request.Adapt(tourguid);
        
        if (request.AllLangues != null)
        {
            var langues = db.Langues
            .Where(i => i.TourguidId == id);
            db.Langues.RemoveRange(langues);
            db.Langues.UpdateRange(langues); 
            foreach (var langue in request.AllLangues)
            {
                var langues1 = new Langues
                {
                    TourguidId = id,
                    Langue = langue
                };
                await db.Langues.AddAsync(langues1, cancellationToken);
            }
        }

        db.Users.Update(tourguid);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> RemoveTourist (string tourguidid , string Touristid , CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == Touristid && i.TourguidId == tourguidid);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == tourguidid);
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        if ((tourguid.Score - 1) > 0)
            tourguid.Score -= 1;
        else
            tourguid.Score = 0;
        tourguid.CurrentTouristsCount -= 1;
        user.TourguidId = null;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<TourguidPublicProfile>> PublicProfile(string id, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users
            .Include(i => i.InverseTourguid)
            .Include(i => i.TourguidAndPlaces).ThenInclude(i => i.Place)
            .Include(i => i.Program).ThenInclude(p => p!.PlaceNames)
            .Include(i => i.Langues)
            .SingleOrDefaultAsync(i => i.Id == id && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure<TourguidPublicProfile>(TourguidErrors.TourguidNotFound);
        var result = tourguid.Adapt<TourguidPublicProfile>();
        result.tourists = tourguid.InverseTourguid.Adapt<List<Tourist>>();
        result.TouristsCount = result.tourists.Count();
        result.RateGroup = db.Tourguid_Rates
            .Where(i => i.tourguidId == id).GroupBy(i => i.rate)
            .Select(i => new RateGroup
            {
                value = i.Key,
                count = i.Count()
            }).ToList();
        //result.Rate = db.Tourguid_Rates
        //    .Where(i => i.tourguidId == id)
        //    .Select(i => i.rate).Count() == 0 ? 0 : (decimal)db.Tourguid_Rates
        //    .Where(i => i.tourguidId == id)
        //    .Select(i => i.rate).Average();

        var rate = db.Tourguid_Rates
            .Where(i => i.tourguidId == id)
            .Select(i => i.rate);
        result.Rate = rate.Count() == 0 ? 0 : Math.Round((decimal)rate.Average(), 1);
        result.AllLangues = await db.Langues
            .Where(i => i.TourguidId == id).AsNoTracking()
            .Select(i => i.Langue).ToListAsync(cancellationToken);
        //result.Rate = tourguid.Tourguid_Rates.Count() == 0 ? 0 : (decimal)tourguid.Tourguid_Rates.Average(i => i.rate);
        if (tourguid.TripName == null)
            result.places = tourguid.TourguidAndPlaces.Select(i => i.Place).Adapt<List<placesinfo>>().ToList();
        // result.placeName[0] = tourguid.TourguidAndPlaces.Select(i => i.PlaceName).First();
        else
            result.TripName = tourguid.TripName;
        //result.places = tourguid.Program!.PlaceNames.Adapt<List<placesinfo>>().ToList();
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

    public async Task<Result> MoveToTripe(string tourguidid, string TripeName, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.SingleOrDefaultAsync
            (i => i.Id == tourguidid &&  i.Role == "Tourguid" , cancellationToken);
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        var Tripe = await db.Trips.SingleOrDefaultAsync(i => i.Name == TripeName , cancellationToken);
        if (Tripe is null)
            return Result.Failure(ProgramErorr.ProgramNotFound);

        tourguid.MoveToTrip = TripeName;

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateMaxTourists(string id, int maxTourists, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.FindAsync(id);
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        tourguid.MaxTourists = maxTourists;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> UpdateIsActive(string id, bool isActive, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.FindAsync(id);
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);

        if (isActive == true)
        {
            var Allusers = await db.Users.Where(i => i.TourguidId == id).ToListAsync(cancellationToken);
            foreach (var user in Allusers)
            {
                user.TourguidId = null;
                user.Tourguid = null;
            }
            tourguid.CurrentTouristsCount = 0;
        }

        tourguid.IsActive = isActive;
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

    public async Task<(byte[] fileContent, string ContentType, string fileName)> DownloadFilesAsync(string id, CancellationToken cancellationToken = default)
    {
        var file = await db.Users.FindAsync(id);

        if (file is null)
            return ([], string.Empty, string.Empty);

        var path = Path.Combine(_filesPath, file.CV!);

        MemoryStream memoryStream = new();
        using FileStream fileStream = new(path, FileMode.Open);
        fileStream.CopyTo(memoryStream);

        memoryStream.Position = 0;

        return (memoryStream.ToArray(), file.CvContentType!, file.CV!);
    }

    public async Task<Result> CreateAcount(AddTourguidRequest request, CancellationToken cancellationToken = default)
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
            // add Trip or place
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
            // add Langues
            Langues langues = new Langues();
            if (request.AllLangues != null)
            {
                foreach (var langue in request.AllLangues)
                {
                    langues = new Langues
                    {
                        TourguidId = tourguid.Id,
                        Langue = langue
                    };
                    await db.Langues.AddAsync(langues, cancellationToken);
                }
            }
            // upload image
            var path = Path.Combine(_imagesPath, request.image.FileName);
            if (tourguid.Photo == null)
            {
                using var stream = File.Create(path);
                await request.image.CopyToAsync(stream, cancellationToken);
            }
            else
            { 
                var oldPath = Path.Combine(_imagesPath, tourguid.Photo);
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
                using var stream = File.Create(path);
                await request.image.CopyToAsync(stream, cancellationToken);
            }
            tourguid.Photo = request.image.FileName;
            tourguid.ContentType = request.image.ContentType;

            /// upload cv
           // var randomFileName = Path.GetRandomFileName();

           

            var path2 = Path.Combine(_filesPath, request.Cvfile.FileName);

            using var stream2 = File.Create(path2);
            await request.Cvfile.CopyToAsync(stream2, cancellationToken);
            tourguid.CV = request.Cvfile.FileName;
            tourguid.CvContentType = request.Cvfile.ContentType;
           // tourguid.StoredFileName = randomFileName;


            await db.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }

        return Result.Failure(UserErrors.notsaved);
    }
   
    // display all trips
    public async Task<Result<AllTripsResponse>> DisplayAllTrips(CancellationToken cancellationToken = default)
    {
        var result = new AllTripsResponse();
        result.Trips = db.Trips.Select(i => i.Adapt<TripsResponse>()).ToList();
        return Result.Success(result);
    }

}
