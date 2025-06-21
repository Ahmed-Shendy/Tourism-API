using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Tourism_Api.Abstractions;
using Tourism_Api.Entity.Admin;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Programs;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.user;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

   
public class AdminServices(TourismContext db, ILogger<AdminServices> logger, IWebHostEnvironment webHostEnvironment,
    UserManager<User> user ,  HybridCache cache , IEmailService emailService) : IAdminServices
{
    private readonly TourismContext db = db;
    private readonly ILogger<AdminServices> logger = logger;
    private readonly UserManager<User> UserMander = user;
    private readonly HybridCache cache = cache;
    private readonly IEmailService _emailService = emailService;
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";
    private readonly string _filesPath = $"{webHostEnvironment.WebRootPath}/files";

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
        // use this to remove the cache
        await cache.RemoveAsync($"AllPlaces");
       
        await db.SaveChangesAsync(cancellationToken);
        var result = place.Adapt<PlacesDetails>();
        result.TypeOfTourism = request.TypeOfTourism;
        return Result.Success(result);
    }

    public async Task<Result> DeletePlace(string name, CancellationToken cancellationToken = default)
    {
        name = name.Replace("%20", " ");// for example if the name is "Ahmed Ayman" it will be "Ahmed%20Ayman"
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == name);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
        db.Places.Remove(place);
        db.Type_of_Tourism_Places.RemoveRange(db.Type_of_Tourism_Places.Where(i => i.Place_Name == name));
        // use this to remove the cache
        await cache.RemoveAsync($"AllPlaces");
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> UpdatePlace(string PlaceName , UpdatePlaceRequest request, CancellationToken cancellationToken = default)
    {
        PlaceName = PlaceName.Replace("%20", " "); // for example if the PlaceName is "Beach%20Resort" it will be "Beach Resort"
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == PlaceName);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
        place = request.Adapt(place);
        db.Places.Update(place);
        await db.SaveChangesAsync(cancellationToken);
        // use this to remove the cache
        await cache.RemoveAsync($"AllPlaces");
        return Result.Success();
    }
    public async Task<Result<AllNotActiveTourguid>> NotActiveTourguid( CancellationToken cancellationToken = default)
    {
        var result = new AllNotActiveTourguid();
        result.NotActiveTourguids = await db.Users.Where(i => i.Role == "Tourguid" 
        && i.EmailConfirmed == false).AsNoTracking()
            .Select(i => new NotActiveTourguids
            {
                Id = i.Id,
                Name = i.Name,
                Photo = i.Photo,
                CV = i.CV
            }).ToListAsync(cancellationToken);
        result.Count = result.NotActiveTourguids.Count;
        return Result.Success(result);
    }
    public async Task<Result> DeleteTourguid(string id, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == id && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        if (tourguid.Phone != null)
        {
            var path = Path.Combine(_imagesPath, tourguid.Photo!);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        if (tourguid.CV != null)
        {
            var cvPath = Path.Combine(_filesPath, tourguid.CV!);
            if (File.Exists(cvPath))
            {
                File.Delete(cvPath);
            }
        }
        db.Users.Remove(tourguid);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
   
    //public async Task<Result> AddTourguidPlace(string tourguidId, string placeName, CancellationToken cancellationToken = default)
    //{
    //    placeName = placeName.Replace("%20", " "); // for example if the placeName is "Beach%20Resort" it will be "Beach Resort"
    //    var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == tourguidId && i.Role == "Tourguid");
    //    if (tourguid is null)
    //        return Result.Failure(TourguidErrors.TourguidNotFound);
    //    var place = await db.Places.SingleOrDefaultAsync(i => i.Name == placeName);
    //    if (place is null)
    //        return Result.Failure(PlacesErrors.PlacesNotFound);
    //    if (tourguid.TripName != null)
    //    {
    //        var cleanTourguid = await db.Users
    //            .Where(i => i.TourguidId == tourguidId).ToListAsync(cancellationToken);
    //        if (cleanTourguid.Any())
    //        {
    //            foreach (var user in cleanTourguid)
    //            {
    //                user.TourguidId = null;
    //            }
    //        }
    //        tourguid.TripName = null;
    //        await db.SaveChangesAsync(cancellationToken);
    //    }

    //    // use it if you want to check if the place is already added to the tourguid
    //    //var ChecktourguidPlace = await db.TourguidAndPlaces.SingleOrDefaultAsync(i => i.PlaceName == placeName && i.TouguidId == tourguidId);
    //    //if (ChecktourguidPlace is not null)
    //    //{

    //    //    return Result.Failure(TourguidErrors.TourguidPlaces);
    //    //}
    //    //var tourguidPlace = new TourguidAndPlaces
    //    //{
    //    //    PlaceName = place.Name,
    //    //    TouguidId = tourguid.Id
    //    //};
    //    //await db.TourguidAndPlaces.AddAsync(tourguidPlace, cancellationToken);
    //    //await db.SaveChangesAsync(cancellationToken);
    //    //return Result.Success();


    //    // use it if you want to the tourguid added one of the place
    //    var ChecktourguidPlace = await db.TourguidAndPlaces.SingleOrDefaultAsync(i => i.TouguidId == tourguidId);
    //    if (ChecktourguidPlace is not null)
    //    {
    //        //db.TourguidAndPlaces.Remove(ChecktourguidPlace);
    //        //await db.SaveChangesAsync(cancellationToken);

    //        var cleanTourguid = await db.Users
    //            .Where(i => i.TourguidId == tourguidId).ToListAsync(cancellationToken);
    //        if (cleanTourguid.Any())
    //        {
    //            foreach (var user in cleanTourguid)
    //            {
    //                user.TourguidId = null;
    //            }
    //            await db.SaveChangesAsync(cancellationToken);
    //        }
    //        ChecktourguidPlace.PlaceName = placeName;
    //        ChecktourguidPlace.MoveToPlace = null;
    //    }
    //    else
    //    {
    //        var tourguidPlace = new TourguidAndPlaces
    //        {
    //            PlaceName = place.Name,
    //            TouguidId = tourguid.Id
    //        };
    //        await db.TourguidAndPlaces.AddAsync(tourguidPlace, cancellationToken);
    //    }
    //    await db.SaveChangesAsync(cancellationToken);
    //    return Result.Success();
    //}

    public async Task<Result> ActiveTourguid(string id, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == id && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        tourguid.EmailConfirmed = true;
        await db.SaveChangesAsync(cancellationToken);
        try
        {
            var emailSubject = "Your TourGuid Account Has Been Activated";
            var emailBody = $"Dear {tourguid.Name},\n\nYour TourGuid account has been successfully activated. You can now log in and start using our platform.\n\nBest regards,\nThe Team";

            await emailService.SendEmailAsync(tourguid.Email, emailSubject, emailBody);
        }
        catch (Exception ex)
        {
            // يمكنك تسجيل الخطأ أو التعامل معه حسب احتياجاتك
            // لكن لا نريد أن يفشل التفعيل إذا فشل إرسال البريد
            logger.LogError(ex, "Failed to send activation email to tourguid {Id}", id);
        }
        return Result.Success();
    }

    public async Task<Result> DeleteTourguidPlace(string tourguidId, string placeName, CancellationToken cancellationToken = default)
    {
        placeName = placeName.Replace("%20", " "); // for example if the placeName is "Beach%20Resort" it will be "Beach Resort"
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
        DachshundResult.allTourguidsByScope = await db.Users.Include(i => i.TourguidAndPlaces).ThenInclude(i => i.Place)
           .Where(i => i.Role == "Tourguid").OrderByDescending(i => i.Score)
           .Select(i => new AllTourguids
           {
               Id = i.Id,
               Email = i.Email,
               Name = i.Name,
               Photo = i.Photo,
               BirthDate = i.BirthDate,
               Gender = i.Gender,
               countOfTourisms = i.Score
               //countOfTourisms = db.Users.Where(UserTourguid => UserTourguid.TourguidId == i.Id).Count()
           }).Take(5).ToListAsync(cancellationToken);


        DachshundResult.CountFamle =  db.Users.Where(i => i.Gender == "Female" || i.Gender == "female").Count();
        DachshundResult.CountMale = db.Users.Where(i => i.Gender == "Male" || i.Gender == "male").Count();
        DachshundResult.CountTourguid = db.Users.Where(i => i.Role == "Tourguid" && i.EmailConfirmed).Count();
        DachshundResult.peopleForCountries = await db.Users.Where(List => List.Role == "User")
            .GroupBy(u => u.Country)
            .Select(g => new PeopleForCountry
            {
                country = g.Key!,
                count = g.Count()
            }).OrderByDescending(i => i.count).ToListAsync(cancellationToken);

        DachshundResult.topFavoritePlaces = await db.FavoritePlaces.Include(i => i.Place)
            .GroupBy(i => i.Place.Name)
            .Select(g => new TopFavoritePlace
            {
                Name = g.Key!,
                GoogleRate = g.FirstOrDefault()!.Place.GoogleRate,
                Photo = g.FirstOrDefault()!.Place.Photo
            }).OrderByDescending(i => i.GoogleRate).Take(5).ToListAsync(cancellationToken);

        return Result.Success(DachshundResult);
    }

    public async Task<Result<TransferRequests>> TransferRequest( CancellationToken cancellationToken = default)
    {
        var allTourguid = await db.Users.Include(i => i.TourguidAndPlaces).ThenInclude(i => i.Place)
            .Where(i => i.Role == "Tourguid" && i.MoveTo != null)
            .Select(i => new TourguidTransferRequest
            {
                TourguidId = i.Id,
                TourguidPhoto = i.Photo,
                TourguidName = i.Name,
                Movedto = i.MoveTo!,
            })
            .ToListAsync(cancellationToken);



        var result = new TransferRequests
        {
            tourguidTransfer = allTourguid,
            Count = allTourguid.Count
        };
        return Result.Success(result);
    }
 
    public async Task<Result> TransferRequestDecline(string tourguidId , CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.
            SingleOrDefaultAsync(i => i.Id == tourguidId && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
       
        tourguid.MoveTo = null;
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

    public async Task<Result> MoveTourguidAccapt(TourguidTrips request, CancellationToken cancellationToken = default)
    {
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == request.TourguidId && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);

        //if (tourguid.MoveTo == null)
        //    return Result.Failure(TourguidErrors.TourguidMoveToNull);

        var trip = await db.Trips.SingleOrDefaultAsync(i => i.Name == request.MoveTo);
        //if (trip is null)
        //    return Result.Failure(ProgramErorr.ProgramNotFound);
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == request.MoveTo);

        var cleanTourguid = await db.Users
               .Where(i => i.TourguidId == tourguid.Id).ToListAsync(cancellationToken);
        if (cleanTourguid.Any())
        {
            foreach (var user in cleanTourguid)
            {
                user.TourguidId = null;
            }
            await db.SaveChangesAsync(cancellationToken);
        }

        var tourguidplace = await db.TourguidAndPlaces.SingleOrDefaultAsync(i => i.TouguidId == request.TourguidId);

        // if the tourguid has a place or trip then remove it
        if (tourguidplace != null || tourguid.TripName != null)
        {
           
            if (tourguidplace != null)
            {
                
                db.TourguidAndPlaces.Remove(tourguidplace);

            }
            if (tourguid.TripName != null)
            {
                tourguid.TripName = null;
            }
            await db.SaveChangesAsync(cancellationToken);
        }
        if (trip != null) { 
            tourguid.TripName = request.MoveTo;
            
        }
        else
        {
            var tourguidPlace = new TourguidAndPlaces
            {
                TouguidId = tourguid.Id,
                PlaceName = request.MoveTo!
            };
            await db.TourguidAndPlaces.AddAsync(tourguidPlace, cancellationToken);
        }
        tourguid.MoveTo = null;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    //public async Task<Result<List<ContactUsProblemDto>>> GetAllContactUsProblems(CancellationToken cancellationToken = default)
    //{
    //    var problems = await db.ContactUs
    //        .Include(c => c.User)
    //        .Select(c => new ContactUsProblemDto
    //        {
    //            Id = c.Id,
    //            Problem = c.Problem,
    //            UserId = c.UserId,
    //            UserName = c.User.Name,
    //            UserEmail = c.User.Email,
    //            UserPhoto = c.User.Photo
    //        })
    //        .ToListAsync(cancellationToken);

    //    return Result.Success(problems);
    //}

    public async Task<Result> ReplyToContactUs(AdminReplyContactUsRequest request, CancellationToken cancellationToken = default)
    {
        var contact = await db.ContactUs.Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == request.ContactUsId, cancellationToken);

        if (contact == null)
            return Result.Failure(new Error("NotFound", "Problem not found.", 404));

        // إرسال الرد على الإيميل
        await _emailService.SendEmailAsync(contact.User.Email, "Reply to your problem", request.ReplyMessage);

        //   بعد الرد
        contact.IsResolved = true;
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    // get all contact us problems not resolved
    public async Task<Result<ContactUsProblemsResponse>> GetAllContactUsProblems(CancellationToken cancellationToken = default)
    {
        var problems = await db.ContactUs
            .Include(c => c.User)
            .Where (c => c.IsResolved == false) // فقط المشاكل غير المحلولة
            .Select(c => new ContactUsProblemDto
            {
                Id = c.Id,
                Problem = c.Problem,
                UserId = c.UserId,
                UserName = c.User.Name,
                UserEmail = c.User.Email,
                UserPhoto = c.User.Photo,
                CreatedAt = c.CreatedAt,
                IsResolved = c.IsResolved
            })
            .ToListAsync(cancellationToken);

        var response = new ContactUsProblemsResponse
        {
            Count = problems.Count,
            Problems = problems
        };

        return Result.Success(response);
    }
    // get all contact us problems  resolved
    public async Task<Result<ContactUsProblemsResponse>> GetAllResolvedContactUsProblems(CancellationToken cancellationToken = default)
    {
        var problems = await db.ContactUs
            .Include(c => c.User)
            .Where(c => c.IsResolved == true) // فقط المشاكل المحلولة
            .Select(c => new ContactUsProblemDto
            {
                Id = c.Id,
                Problem = c.Problem,
                UserId = c.UserId,
                UserName = c.User.Name,
                UserEmail = c.User.Email,
                UserPhoto = c.User.Photo,
                CreatedAt = c.CreatedAt,
                IsResolved = c.IsResolved
            })
            .ToListAsync(cancellationToken);
        var response = new ContactUsProblemsResponse
        {
            Count = problems.Count,
            Problems = problems
        };
        return Result.Success(response);
    }




    // delete user problem 
    public async Task<Result> DeleteContactUsProblem(int problemId, CancellationToken cancellationToken = default)
    {
        var problem = await db.ContactUs.FindAsync(problemId);
        if (problem == null)
            return Result.Failure(new Error("NotFound", "Problem not found.", 404));
        db.ContactUs.Remove(problem);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    
    public async Task<Result> AddTrip(AddTripRequest request, CancellationToken cancellationToken = default)
    {
        var tripExists = await db.Trips.AnyAsync(t => t.Name == request.Name, cancellationToken);
        if (tripExists)
            return Result.Failure(ProgramErorr.ProgramUnque);
        var ProgramExists = await db.Programs.AnyAsync(p => p.Name == request.programName, cancellationToken);
        if (!ProgramExists)
            return Result.Failure(ProgramErorr.ProgramNotFound);


        Trips trip = new Trips
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Days = request.Days,
            programName = request.programName,
            Number_of_Sites = request.TripsPlaces.Count
        };

        await db.Trips.AddAsync(trip, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        // add trip places
        foreach (var placeName in request.TripsPlaces)
        {
            var place = await db.Places.SingleOrDefaultAsync(p => p.Name == placeName, cancellationToken);
            if (place != null)
            {
                var tripPlace = new TripsPlaces
                {
                    TripName = trip.Name,
                    PlaceName = place.Name
                };
                await db.TripsPlaces.AddAsync(tripPlace, cancellationToken);
            }
            else
            {
                // If a place in the request does not exist, you might want to handle it (e.g., log an error or throw an exception)
                logger.LogWarning("Place {PlaceName} not found for trip {TripName}", placeName, trip.Name);
            }
        }
        await db.SaveChangesAsync(cancellationToken);

        // Remove cache for updated trips list
        await cache.RemoveAsync("AllTrips"); 

        return Result.Success();
    }

    public async Task<Result> UpdateTrip(string tripName, UpdateTripRequest request, CancellationToken cancellationToken = default)
    {
        tripName = tripName.Replace("%20", " "); // for example if the tripName is "Beach%20Trip" it will be "Beach Trip"
        var trip = await db.Trips.SingleOrDefaultAsync(t => t.Name == tripName, cancellationToken);
        if (trip == null)
            return Result.Failure(ProgramErorr.ProgramNotFound);

        var ProgramExists = await db.Programs.AnyAsync(p => p.Name == request.programName, cancellationToken);
        if (!ProgramExists)
            return Result.Failure(ProgramErorr.ProgramNotFound);

        trip.Number_of_Sites = request.Trips_Places.Count;
        trip = request.Adapt(trip);
        db.Trips.Update(trip);
        await db.SaveChangesAsync(cancellationToken);

        // Update trip places
        // First, remove existing trip places
        var existingTripPlaces = await db.TripsPlaces.Where(tp => tp.TripName == tripName).ToListAsync(cancellationToken);
        db.TripsPlaces.RemoveRange(existingTripPlaces);
        await db.SaveChangesAsync(cancellationToken);
        // Then, add new trip places
        foreach (var placeName in request.Trips_Places)
        {
            var place = await db.Places.SingleOrDefaultAsync(p => p.Name == placeName, cancellationToken);
            if (place != null)
            {
                var tripPlace = new TripsPlaces
                {
                    TripName = trip.Name,
                    PlaceName = place.Name
                };
                await db.TripsPlaces.AddAsync(tripPlace, cancellationToken);
            }
            else
            {
                // If a place in the request does not exist, you might want to handle it (e.g., log an error or throw an exception)
                logger.LogWarning("Place {PlaceName} not found for trip {TripName}", placeName, tripName);
            }
        }

        await db.SaveChangesAsync(cancellationToken);

        // Remove cache for updated trips list
         await cache.RemoveAsync("AllTrips");

        return Result.Success();
    }

    public async Task<Result> DeleteTrip(string tripName, CancellationToken cancellationToken = default)
    {
        tripName = tripName.Replace("%20", " "); // for example if the tripName is "Beach%20Trip" it will be "Beach Trip"
        var trip = await db.Trips.SingleOrDefaultAsync(t => t.Name == tripName, cancellationToken);
        if (trip == null)
            return Result.Failure(ProgramErorr.ProgramNotFound);

        db.Trips.Remove(trip);
        await db.SaveChangesAsync(cancellationToken);
        

        //var tripPlaces = await db.TripsPlaces.Where(tp => tp.TripName == trip.Name).ToListAsync(cancellationToken);
        //db.TripsPlaces.RemoveRange(tripPlaces);
        //await db.SaveChangesAsync(cancellationToken);

        // Remove cache for updated trips list
        await cache.RemoveAsync("AllTrips");

        return Result.Success();
    }

    public async Task<Result> DeleteAnyComment(int commentId, CancellationToken cancellationToken = default)
    {
        var comment = await db.Comments.SingleOrDefaultAsync(i => i.Id == commentId, cancellationToken);
        if (comment is null)
            return Result.Failure(CommentErrors.CommentNotFound);
        
        db.Comments.Remove(comment);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
