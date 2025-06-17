using Tourism_Api.Abstractions;

namespace Tourism_Api.Errors;

public static class TourguidErrors
{
    public static readonly Error TourguidNotFound =
        new("Tourguid.NotFound", "No Tourguid was found ", StatusCodes.Status404NotFound);

    public static readonly Error TourguidPlaces =
        new("Tourguid.TourguidPlaces", "Tourguid oready in this Place", StatusCodes.Status404NotFound);

    public static readonly Error TourguidMoveToNull =
       new("Tourguid.TourguidMoveToNull", "Tourguid MoveTo Null", StatusCodes.Status404NotFound);

    public static readonly Error MaxTouristsMustBeGreaterThanZero =
        new("Tourguid.MaxTouristsMustBeGreaterThanZero", "Max tourists must be greater than zero", StatusCodes.Status400BadRequest);


}

