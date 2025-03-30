using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using System.Security.Claims;
using System.Xml.Linq;
using Tourism_Api.Entity.Comment;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.user;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

public class UserServices (TourismContext db , HybridCache cache) : IUserServices
{
    private readonly TourismContext db = db;
    private readonly HybridCache cache = cache;

    public async Task<Result<UserRespones>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null)
            return Result.Failure<UserRespones>(UserErrors.UserNotFound);
        return Result.Success(user.Adapt<UserRespones>());
    }

    public async Task<Result> AddComment(string UserId, AddComment request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == request.PlaceName);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);

        var newComment = request.Adapt<Comment>();
        newComment.UserId = UserId;

        await db.Comments.AddAsync(newComment, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteComment(string UserId, int CommentId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var comment = await db.Comments.SingleOrDefaultAsync(i => i.Id == CommentId);
        if (comment is null)
            return Result.Failure(CommentErrors.CommentNotFound);
        if (comment.UserId != UserId)
            return Result.Failure(CommentErrors.UserNotAuthorized);
        db.Comments.Remove(comment);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateComment(string UserId, UpdateComment request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var comment = await db.Comments.SingleOrDefaultAsync(i => i.Id == request.CommentId);
        if (comment is null)
            return Result.Failure(CommentErrors.CommentNotFound);
        if (comment.UserId != UserId)
            return Result.Failure(CommentErrors.UserNotAuthorized);
        comment.Content = request.Content;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> Addrate(string UserId, AddRate request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == request.PlaceName);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
        var rate = await db.placeRates.SingleOrDefaultAsync(i => i.UserId == UserId && i.PlaceName == request.PlaceName);
        if (rate is not null)
        {
            place.Rate -= rate.Rate;
            if ((place.Rate + request.Rate) <= 500000)
                place.Rate += request.Rate;
            rate.Rate = request.Rate;
        }
        else
        {
            var newRate = request.Adapt<PlaceRate>();
            newRate.UserId = UserId;
            await db.placeRates.AddAsync(newRate, cancellationToken);
            if ((place.Rate + request.Rate) <= 500000)
                place.Rate += request.Rate;

        }
        // use this to remove the cache
        await cache.RemoveAsync($"AllPlaces");
        await db.SaveChangesAsync(cancellationToken);        
        return Result.Success();
    }

    public async Task<Result> ReservationTourguid(string UserId, string TourguidId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId && i.Role == "User");
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == TourguidId && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        user.TourguidId = TourguidId;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> CancelReservationTourguid(string UserId , CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId && i.Role == "User");
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        user.TourguidId = null;
        user.Tourguid = null;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<Tourguids>> DisplayReservationTourguid(string UserId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.Include(i=> i.Tourguid).SingleOrDefaultAsync(i => i.Id == UserId && i.Role == "User");
        if (user is null)
            return Result.Failure<Tourguids>(UserErrors.UserNotFound);
        
        //var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == user.TourguidId);
        if (user.Tourguid is null)
            return Result.Failure<Tourguids>(TourguidErrors.TourguidNotFound);
        var result = user.Tourguid.Adapt<Tourguids>();
        return Result.Success(result);
    }

    public async Task<Result<Profile>> UserProfile(string UserId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.Include(i => i.Tourguid).Include(i => i.FavoritePlaces).ThenInclude(i => i.Place)
            .SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure<Profile>(UserErrors.UserNotFound);
        var result = user.Adapt<Profile>();
        result.Tourguid = user.Tourguid.Adapt<tourguidinfo>();
        result.FavoritePlaces = user.FavoritePlaces.
            Select(i => new FavPlaces { Name = i.Place.Name , Photo = i.Place.Photo }).ToList();
        return Result.Success(result);
    }

    public async Task<Result> UpdateProfile(string UserId, ProfileUpdate request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.FindAsync(UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        user = request.Adapt(user);
        db.Users.Update(user);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> AddFavoritePlace(string UserId, string PlaceName, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == PlaceName);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
        var favorite = await db.FavoritePlaces.SingleOrDefaultAsync(i => i.UserId == UserId && i.PlaceName == PlaceName);
        if (favorite is not null)
            return Result.Failure(PlacesErrors.PlaceAlreadyFavorite);
        var newFavorite = new FavoritePlace
        {
            UserId = UserId,
            PlaceName = PlaceName
        };
        await db.FavoritePlaces.AddAsync(newFavorite, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> RemoveFavoritePlace(string UserId, string PlaceName, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var favorite = await db.FavoritePlaces.SingleOrDefaultAsync(i => i.UserId == UserId && i.PlaceName == PlaceName);
        if (favorite is null)
            return Result.Failure(PlacesErrors.PlaceNotFavorite);
        db.FavoritePlaces.Remove(favorite);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

}