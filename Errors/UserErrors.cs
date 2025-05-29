using Tourism_Api.Abstractions;

namespace Tourism_Api.Errors;

public static class UserErrors
{
    public static readonly Error UserNotFound =
        new("User.NotFound", "No User was found ", StatusCodes.Status404NotFound);


    public static readonly Error EmailUnque =
        new("User.EmailUnque", "Email must unique", StatusCodes.Status400BadRequest);

    public static readonly Error LookUser =
       new("User.LookUser", "Looked user For 5 Minutes", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidToken =
       new("User.InvalidToken", "Token is not Right", StatusCodes.Status404NotFound);

    public static readonly Error notsaved =
       new("User.notsaved", "Enter right Date", StatusCodes.Status400BadRequest);

    public static readonly Error EmailNotConfirmed =
       new("User.EmailNotConfirmed", "your Email Not Confirmed From Admin", StatusCodes.Status404NotFound);
    public static readonly Error InvalidCode =
       new("User.InvalidCode", "Invalid Code", StatusCodes.Status404NotFound);

    public static readonly Error EmailAlreadyExists =
        new("User.AlreadyExists", "Email with this email already exists", StatusCodes.Status400BadRequest);


}

