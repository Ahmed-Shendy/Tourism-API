using Tourism_Api.Abstractions;

namespace Tourism_Api.Errors;

public static class CommentErrors
{
    public static readonly Error CommentNotFound =
        new("comment.CommentNotFound", "No comment was found ", StatusCodes.Status404NotFound);

    public static readonly Error UserNotAuthorized =
      new("comment.UserNotAuthorized", "User Not Authorized", StatusCodes.Status400BadRequest);
   


}

