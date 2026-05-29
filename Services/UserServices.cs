using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
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

public class UserServices (TourismContext db , HybridCache cache , UserManager<User> user) : IUserServices
{
    private readonly TourismContext db = db;
    private readonly HybridCache cache = cache;
    private readonly UserManager<User> _userManager = user;

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
            if ((place.Rate + request.Rate) <= decimal.Max(5,2))
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
        user.Bocked_Date = DateTime.Now;
        if (( tourguid.Score + 1) < ulong.MaxValue)
            tourguid.Score += 1;
        else
            tourguid.Score = ulong.MaxValue;
        if (tourguid.MaxTourists != null) {
            if ((tourguid.CurrentTouristsCount + 1) > tourguid.MaxTourists)
                tourguid.IsActive = false;
            else if ((tourguid.CurrentTouristsCount + 1) == tourguid.MaxTourists){
                tourguid.IsActive = false;
                tourguid.CurrentTouristsCount += 1;
            }
            else
                tourguid.CurrentTouristsCount += 1;

        }
        else
            tourguid.CurrentTouristsCount += 1;

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> CancelReservationTourguid(string UserId , CancellationToken cancellationToken = default)
    {
        var user = await db.Users.Include(i => i.Tourguid).SingleOrDefaultAsync(i => i.Id == UserId && i.Role == "User");
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        if (user.Tourguid != null)
        {
            if ((user.Tourguid.Score - 1) > 0)
                user.Tourguid.Score -= 1;
            else
                user.Tourguid.Score = 0;
        }
        user.Tourguid!.CurrentTouristsCount -= 1;
        user.TourguidId = null;
        user.Tourguid = null;
        user.Bocked_Date = null;

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
        var user = await db.Users
        .Include(i => i.FavoritePlaces).ThenInclude(i => i.Place)
        .Include(i => i.Tourguid)
            .SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure<Profile>(UserErrors.UserNotFound);
        var result = user.Adapt<Profile>();
        if (user.Tourguid != null)
        {
            result.Tourguid = user.Tourguid.Adapt<tourguidinfo>();
           
            var rate = db.Tourguid_Rates
                .Where(i => i.tourguidId == user.Tourguid.Id)
                .Select(i => i.rate);
            result.Tourguid.Rate = rate.Count() == 0 ? 0 : Math.Round((decimal)rate.Average(), 1);
        }
        else
        {
            result.Tourguid = null;
        }
        if (user.FavoritePlaces != null)
        {
            result.FavoritePlaces = user.FavoritePlaces.
                Select(i => new FavPlaces { Name = i.Place.Name , Photo = i.Place.Photo }).ToList();
        }
        else
        {
            result.FavoritePlaces = new List<FavPlaces>();
        }
        return Result.Success(result);
    }

    public async Task<Result<Public_Profile>> PublicProfile(string UserId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.Include(i => i.Tourguid)
            .SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure<Public_Profile>(UserErrors.UserNotFound);
        if (user.Tourguid != null)
        {
            var result = user.Adapt<Public_Profile>();
            
             var rate = db.Tourguid_Rates
                .Where(i => i.tourguidId == user.Tourguid.Id)
                .Select(i => i.rate);
            result.Tourguid!.Rate = rate.Count() == 0 ? 0 : Math.Round((decimal)rate.Average(), 1);
            return Result.Success(result);
        }
        else
        {
            var result = user.Adapt<Public_Profile>();
            return Result.Success(result);
        }
        
        
    }

    public async Task<Result> UpdateProfile(string UserId, ProfileUpdate request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.FindAsync(UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        
        var EmailCheck = await db.Users.AnyAsync(i => i.Email == request.Email && i.Id != UserId);
        if (EmailCheck)
            return Result.Failure(UserErrors.EmailAlreadyExists);
       

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // إعادة تعيين كلمة المرور باستخدام Token
        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
        if (!result.Succeeded)
            return Result.Failure(UserErrors.notsaved);

        user = request.Adapt(user);
        db.Users.Update(user);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> AddOrRemoveFavoritePlace(string UserId, string PlaceName, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var place = await db.Places.SingleOrDefaultAsync(i => i.Name == PlaceName);
        if (place is null)
            return Result.Failure(PlacesErrors.PlacesNotFound);
        var favorite = await db.FavoritePlaces.SingleOrDefaultAsync(i => i.UserId == UserId && i.PlaceName == PlaceName);
        if (favorite is not null)
        {
            db.FavoritePlaces.Remove(favorite);
            await db.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        else
        {
            var newFavorite = new FavoritePlace
            {
                UserId = UserId,
                PlaceName = PlaceName
            };
            await db.FavoritePlaces.AddAsync(newFavorite, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }

    // public async Task<Result> RemoveFavoritePlace(string UserId, string PlaceName, CancellationToken cancellationToken = default)
    // {
    //     var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
    //     if (user is null)
    //         return Result.Failure(UserErrors.UserNotFound);
    //     var favorite = await db.FavoritePlaces.SingleOrDefaultAsync(i => i.UserId == UserId && i.PlaceName == PlaceName);
    //     if (favorite is null)
    //         return Result.Failure(PlacesErrors.PlaceNotFavorite);
    //     db.FavoritePlaces.Remove(favorite);
    //     await db.SaveChangesAsync(cancellationToken);
    //     return Result.Success();
    // }

    public async Task<Result> AddTourguidRate(string UserId, AddTourguidRate request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId && i.Role == "User");
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var tourguid = await db.Users.SingleOrDefaultAsync(i => i.Id == request.tourguidId && i.Role == "Tourguid");
        if (tourguid is null)
            return Result.Failure(TourguidErrors.TourguidNotFound);
        var rate = await db.Tourguid_Rates.SingleOrDefaultAsync(i => i.userId == UserId && i.tourguidId == request.tourguidId);
        if (rate is not null)
        {
            rate.rate = 0;
            rate.rate = request.rate;
            
        }
        else
        {
            var newRate = new Tourguid_Rate
            {
                userId = UserId,
                rate = request.rate,
                tourguidId = request.tourguidId,
            };
            
            await db.Tourguid_Rates.AddAsync(newRate, cancellationToken);
            
        }
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> SendContactUsProblem(string userId, UserProblem userProblem, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.FindAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var contactUs = new ContactUs
        {
            UserId = userId,
            Problem = userProblem.Problem,
        };
        await db.ContactUs.AddAsync(contactUs, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    // Get user program 
    public async Task<Result<string>> RecomendProgram(string userid , CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == userid  );
        if (user is null)
            return Result.Failure<string>(UserErrors.UserNotFound);

        var program = await db.UserProgram.SingleOrDefaultAsync(i => i.UserId == user.Id);
        if (program is null)
            return Result.Failure<string>(ProgramErorr.ProgramNotFound);
        return Result.Success(program.ProgramName);

    }

    // Like or Unlike a comment (Toggle)
    public async Task<Result> LikeOrUnlikeComment(string UserId, int CommentId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var comment = await db.Comments.SingleOrDefaultAsync(i => i.Id == CommentId);
        if (comment is null)
            return Result.Failure(CommentErrors.CommentNotFound);

        var existingLike = await db.CommentLikes
            .SingleOrDefaultAsync(i => i.CommentId == CommentId && i.UserId == UserId, cancellationToken);

        if (existingLike is not null)
        {
            // Unlike - remove existing like
            db.CommentLikes.Remove(existingLike);
        }
        else
        {
            // Like - add new like
            var newLike = new CommentLike
            {
                CommentId = CommentId,
                UserId = UserId,
                CreatedAt = DateTime.UtcNow
            };
            await db.CommentLikes.AddAsync(newLike, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    // Reply to a comment
    public async Task<Result> ReplyToComment(string UserId, ReplyComment request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var parentComment = await db.Comments.SingleOrDefaultAsync(i => i.Id == request.ParentCommentId);
        if (parentComment is null)
            return Result.Failure(CommentErrors.ParentCommentNotFound);

        var reply = new CommentReply
        {
            Content = request.Content,
            UserId = UserId,
            CommentId = request.ParentCommentId,
            CreatedAt = DateTime.UtcNow
        };

        await db.CommentReplies.AddAsync(reply, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    // Get replies for a comment
    public async Task<Result<List<UserComment>>> GetCommentReplies(string? UserId, int CommentId, CancellationToken cancellationToken = default)
    {
        var comment = await db.Comments.SingleOrDefaultAsync(i => i.Id == CommentId);
        if (comment is null)
            return Result.Failure<List<UserComment>>(CommentErrors.CommentNotFound);

        var replies = await db.CommentReplies
            .Include(i => i.User)
            .Include(i => i.Likes)
            .Where(i => i.CommentId == CommentId)
            .Select(i => new UserComment
            {
                Content = i.Content,
                UserName = i.User.Name,
                photo = i.User.Photo,
                UserId = i.UserId,
                id = i.Id,
                LikesCount = i.Likes.Count,
                IsLikedByCurrentUser = UserId != null && i.Likes.Any(l => l.UserId == UserId),
                CreatedAt = i.CreatedAt,
                Replies = null
            })
            .OrderBy(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

        return Result.Success(replies);
    }

    // Like or Unlike a comment reply (Toggle)
    public async Task<Result> LikeOrUnlikeCommentReply(string UserId, int ReplyId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId, cancellationToken);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var reply = await db.CommentReplies.SingleOrDefaultAsync(i => i.Id == ReplyId, cancellationToken);
        if (reply is null)
            return Result.Failure(CommentErrors.ReplyNotFound);

        var existingLike = await db.CommentReplyLikes
            .SingleOrDefaultAsync(i => i.ReplyId == ReplyId && i.UserId == UserId, cancellationToken);

        if (existingLike is not null)
        {
            // Unlike - remove existing reply like
            db.CommentReplyLikes.Remove(existingLike);
        }
        else
        {
            // Like - add new reply like
            var newLike = new CommentReplyLike
            {
                ReplyId = ReplyId,
                UserId = UserId,
                CreatedAt = DateTime.UtcNow
            };
            await db.CommentReplyLikes.AddAsync(newLike, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    // Reply to a reply (nested reply)
    public async Task<Result> ReplyToReply(string UserId, ReplyToReplyComment request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId, cancellationToken);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var parentReply = await db.CommentReplies.SingleOrDefaultAsync(i => i.Id == request.ParentReplyId, cancellationToken);
        if (parentReply is null)
            return Result.Failure(CommentErrors.ReplyNotFound);

        var nestedReply = new CommentReply
        {
            Content = request.Content,
            UserId = UserId,
            CommentId = parentReply.CommentId,  // Keep reference to original comment
            ParentReplyId = request.ParentReplyId,
            CreatedAt = DateTime.UtcNow
        };

        //va
        await db.CommentReplies.AddAsync(nestedReply, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    // Get nested comment replies recursively
    public async Task<Result<List<NestedUserComment>>> GetNestedCommentReplies(string? UserId, int CommentId, CancellationToken cancellationToken = default)
    {
        var comment = await db.Comments.SingleOrDefaultAsync(i => i.Id == CommentId, cancellationToken);
        if (comment is null)
            return Result.Failure<List<NestedUserComment>>(CommentErrors.CommentNotFound);

        // Get all top-level replies (ParentReplyId is null)
        var topLevelReplies = await db.CommentReplies
            .Include(i => i.User)
            .Include(i => i.Likes)
            .Where(i => i.CommentId == CommentId && i.ParentReplyId == null)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

        var result = new List<NestedUserComment>();

        foreach (var reply in topLevelReplies)
        {
            var nestedComment = await BuildNestedCommentTree(reply, UserId, cancellationToken);
            result.Add(nestedComment);
        }

        return Result.Success(result);
    }

    // Helper method to recursively build nested comment tree
    private async Task<NestedUserComment> BuildNestedCommentTree(CommentReply reply, string? UserId, CancellationToken cancellationToken)
    {
        var nestedComment = new NestedUserComment
        {
            Id = reply.Id,
            Content = reply.Content,
            UserName = reply.User.Name,
            Photo = reply.User.Photo,
            UserId = reply.UserId,
            LikesCount = reply.Likes.Count,
            IsLikedByCurrentUser = UserId != null && reply.Likes.Any(l => l.UserId == UserId),
            CreatedAt = reply.CreatedAt,
            ParentReplyId = reply.ParentReplyId,
            Replies = []
        };

        // Get child replies
        var childReplies = await db.CommentReplies
            .Include(i => i.User)
            .Include(i => i.Likes)
            .Where(i => i.ParentReplyId == reply.Id)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

        // Recursively build child tree
        foreach (var childReply in childReplies)
        {
            var childNestedComment = await BuildNestedCommentTree(childReply, UserId, cancellationToken);
            nestedComment.Replies.Add(childNestedComment);
        }

        return nestedComment;
    }

    // Delete a comment reply (with validation)
    public async Task<Result> DeleteCommentReply(string UserId, int ReplyId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId, cancellationToken);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var reply = await db.CommentReplies.SingleOrDefaultAsync(i => i.Id == ReplyId, cancellationToken);
        if (reply is null)
            return Result.Failure(CommentErrors.ReplyNotFound);

        // Check if user owns the reply
        if (reply.UserId != UserId)
            return Result.Failure(CommentErrors.UserNotAuthorized);

        db.CommentReplies.Remove(reply);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    // Update a comment reply
    public async Task<Result> UpdateCommentReply(string UserId, UpdateCommentReply request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Id == UserId, cancellationToken);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var reply = await db.CommentReplies.SingleOrDefaultAsync(i => i.Id == request.ReplyId, cancellationToken);
        if (reply is null)
            return Result.Failure(CommentErrors.ReplyNotFound);

        // Check if user owns the reply
        if (reply.UserId != UserId)
            return Result.Failure(CommentErrors.UserNotAuthorized);

        reply.Content = request.Content;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}