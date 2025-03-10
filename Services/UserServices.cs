using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;
using Tourism_Api.Entity.Comment;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.user;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

public class UserServices (TourismContext db ) : IUserServices
{
    private readonly TourismContext db = db;

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
        await db.SaveChangesAsync(cancellationToken);        
        return Result.Success();
    }

}