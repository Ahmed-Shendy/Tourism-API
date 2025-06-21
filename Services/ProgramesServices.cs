using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Programs;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

public class ProgramesServices(TourismContext Db) : IProgramesServices
{
    private readonly TourismContext db = Db;

    
    public async Task<Result<List<ALLPlaces>>> RecomendPlaces(string userId ,CancellationToken cancellationToken = default)
    {

        var user = await db.UserProgram.Include(i => i.Program).ThenInclude(i => i.PlaceNames)
            .SingleOrDefaultAsync(i => i.UserId == userId);
        if (user is null)
            return Result.Failure<List<ALLPlaces>>(UserErrors.UserNotFound);
       
        var program = user.Program;

        //var program = await db.Programs.Include(i => i.PlaceNames)
        //    .SingleOrDefaultAsync(i => i.Name == programName);
        if (program is null)
            return Result.Failure<List<ALLPlaces>>(ProgramErorr.ProgramNotFound);

        var result = program.PlaceNames
            .Select(i => new ALLPlaces
            {
                Name = i.Name,
                Photo = i.Photo!,
                GoogleRate = i.GoogleRate!
               
            }).OrderByDescending(i => i.GoogleRate).ToList();

        return result is not null && result.Any()
            ? Result.Success(result)
            : Result.Failure<List<ALLPlaces>>(PlacesErrors.PlacesNotFound);
    }

    public async Task<Result<TripDetails>> TripDetails(string userId , string TripName , CancellationToken cancellationToken = default)
    {
        TripName = TripName.Replace("%20", " ");


        var Trip = await db.Trips
            .Include(i => i.TripsPlaces).ThenInclude(i => i.Place)
            .Include(i => i.Tourguids)
            .SingleOrDefaultAsync(i => i.Name == TripName);
        if (Trip is null)
            return Result.Failure<TripDetails>(ProgramErorr.ProgramNotFound);

        
        TripDetails result = new TripDetails
        {
            Name = Trip.Name,
            Description = Trip.Description,
            Price = Trip.Price,
            Days = Trip.Days,
            Number_of_Sites = Trip.Number_of_Sites,
            programName = Trip.programName,
            TripPlaces = Trip.TripsPlaces.Select(i => i.Place.Adapt<Trip_Places>()).ToList(),
            //Select(i => new TripPlaces
            //{
            //    PlaceName = i.PlaceName,
            //    Photo = i.Place.Photo,
            //}).ToList(),
            Tourguids = Trip.Tourguids.Where(i=> i.EmailConfirmed).Adapt<List<Tourguids>>()
            //Select(i => new Tourguids
            //{
            //    Name = i.Name,
            //    Phone = i.Phone,
            //    Photo = i.Photo,
            //    Id = i.Id,
            //    Email = i.Email
            //}).ToList()
            
        };
        foreach (var tourguid in result.Tourguids)
        {
            var rate = db.Tourguid_Rates
                .Where(i => i.tourguidId == tourguid.Id)
                .Select(i => i.rate);
            tourguid.rate = rate.Count() == 0 ? 0 : Math.Round((decimal)rate.Average(), 1);
            tourguid.IsBooked = await db.Users.
                FirstOrDefaultAsync(x => x.Id == userId && x.TourguidId == tourguid.Id , cancellationToken) != null;
        }

        return Result.Success(result);
    }

    public async Task<Result<List<TripsResponse>>> AllTripsInProgram( string userid , CancellationToken cancellationToken = default)
    {
        var userTrips =  db.UserProgram.
             Include(i => i.Program).ThenInclude(i => i.Trips)
            .SingleOrDefaultAsync(i => i.UserId == userid);
        var trips = userTrips.Result!.Program.Trips
            .Select(i => new TripsResponse
            {
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                Days = i.Days,
                Number_of_Sites = i.Number_of_Sites,

            }).ToList();

        return userTrips is not null && trips.Any()
            ? Result.Success(trips)
            : Result.Failure<List<TripsResponse>>(ProgramErorr.ProgramNotFound);
    }
    public async Task<Result> GetProgram (string userId, string programName, CancellationToken cancellationToken = default)
    {
        programName = programName.Replace("%20", " ");

        var program = await db.Programs.SingleOrDefaultAsync(i => i.Name == programName);
        if (program is null)
            return Result.Failure(ProgramErorr.ProgramNotFound);
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var userProgram = await db.UserProgram
            .SingleOrDefaultAsync(i => i.UserId == userId && i.ProgramName == programName);
        if (userProgram is not null)
        {
            userProgram.ProgramName = program.Name;
            userProgram.UserId = user.Id;
        }
        else
        {
            await db.UserProgram.AddAsync(new UserProgram
            {
                UserId = userId,
                ProgramName = program.Name
            }, cancellationToken);
        }
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

   
    public async Task<Result<List<string>>> Tourism_Type(CancellationToken cancellationToken = default)
    {
        var tourismType = new List<string>
        {
            "Adventure Program",
            "Beach Program",
            "Historical Program",
            "Major Cities Program",
            "Relaxation Program"
        };
        return Result.Success(tourismType);
    }
    public async Task<Result<List<string>>> With_Family(CancellationToken cancellationToken = default)
    {
        var withFamily = new List<string>
        {
            "With Family",
            "Without Family"
        };
        return Result.Success(withFamily);
    }

    public async Task<Result<List<string>>> Accommodation_Type(CancellationToken cancellationToken = default)
    {
        var accommodationType = new List<string>
        {
            "Airbnb",
            "Hostel",
            "Hotel",
            "Resort"
        };
        return Result.Success(accommodationType);
    }
    public async Task<Result<List<string>>> Preferred_Destination(CancellationToken cancellationToken = default)
    {
        var Preferred_Destination = new List<string>
        {
            "Adventure Park",
            "Beach",
            "City",
            "Countryside",
            "Mountain"

        };
        return Result.Success(Preferred_Destination);
    }

    public async Task<Result<List<string>>> Travel_Purpose(CancellationToken cancellationToken = default)
    {
        var travelPurpose = new List<string>
        {
            "Business",
            "Education",
            "Leisure",
            "Medical"
        };
        return Result.Success(travelPurpose);
    }

    public async Task<Result<List<string>>> AllTripsName(CancellationToken cancellationToken)
    {
        var result = await db.Trips.Select(x => x.Name).ToListAsync(cancellationToken);

        return Result.Success(result);
    }

}
