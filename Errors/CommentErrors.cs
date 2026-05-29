using Tourism_Api.Abstractions;

namespace Tourism_Api.Errors;

public static class CommentErrors
{
    public static readonly Error CommentNotFound =
        new("comment.CommentNotFound", "No comment was found ", StatusCodes.Status404NotFound);

    public static readonly Error UserNotAuthorized =
        new("comment.UserNotAuthorized", "User Not Authorized", StatusCodes.Status400BadRequest);
   
    public static readonly Error ParentCommentNotFound =
        new("comment.ParentCommentNotFound", "Parent comment not found", StatusCodes.Status404NotFound);

    public static readonly Error ReplyNotFound =
        new("comment.ReplyNotFound", "No comment reply was found", StatusCodes.Status404NotFound);
}
