using Microsoft.EntityFrameworkCore;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Programs;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

public class ProgramesServices(TourismContext Db) : IProgramesServices
{
    private readonly TourismContext db = Db;

    //public async Task<Result<List<ALLProgrames>>> DisplayAllProgrames(CancellationToken cancellationToken = default)
    //{
    //    var result = await db.Programes
    //        .Select(i => new ALLProgrames { Name = i.Name, Photo = i.Photo ?? "" })
    //        .ToListAsync(cancellationToken);
    //    return result is not null && result.Any()
    //        ? Result.Success(result)
    //        : Result.Failure<List<ALLProgrames>>(ProgramesErrors.ProgramesNotFound);
    //}
    public async Task<Result<List<ALLPlaces>>> RecomendPlaces(string userId ,CancellationToken cancellationToken = default)
    {

        var user = await db.UserAswers.Include(i => i.Program).ThenInclude(i => i.PlaceNames)
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
                Rate = i.Rate!.Value
               
            }).OrderByDescending(i => i.Rate).ToList();

        return result is not null && result.Any()
            ? Result.Success(result)
            : Result.Failure<List<ALLPlaces>>(PlacesErrors.PlacesNotFound);
    }

    public async Task<Result<ProgramDetails>> ProgramDetails(string userId , CancellationToken cancellationToken = default)
    {

        var user = await db.UserAswers.Include(i => i.Program).ThenInclude(i => i.PlaceNames)
            .SingleOrDefaultAsync(i => i.UserId == userId);
        if (user is null)
            return Result.Failure<ProgramDetails>(UserErrors.UserNotFound);

        var program = user.Program;
        var result = new ProgramDetails
        {
            Name = program.Name,
            Description = program.Description,
            Price = program.Price,
            Activities = program.Activities,
            programsPlaces = program.PlaceNames.Select(i => new ProgramsPlaces
            {
                PlaceName = i.Name,
                Photo = i.Photo!
            }).ToList()
        };
        return Result.Success(result);
    }
}
