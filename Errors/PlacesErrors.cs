using Tourism_Api.Abstractions;

namespace Tourism_Api.Errors;

public static class PlacesErrors
{
    public static readonly Error PlacesNotFound =
        new("Places.PlacesNotFound", "No Places was found ", StatusCodes.Status404NotFound);


    //public static readonly Error EmailUnque =
    //    new("User.EmailUnque", "Email must unique", StatusCodes.Status400BadRequest);

    //public static readonly Error LookUser =
    //   new("User.LookUser", "Looked user For 5 Minutes", StatusCodes.Status400BadRequest);

    //public static readonly Error InvalidToken =
    //   new("User.InvalidToken", "Token is not Right", StatusCodes.Status404NotFound);


}
