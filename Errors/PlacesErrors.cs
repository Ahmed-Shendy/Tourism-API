using Tourism_Api.Abstractions;

namespace Tourism_Api.Errors;

public static class PlacesErrors
{
    public static readonly Error PlacesNotFound =
        new("Places.PlacesNotFound", "No Places was found ", StatusCodes.Status404NotFound);

    public static readonly Error PlaceAlreadyFavorite =
        new("Places.PlaceAlreadyFavorite", "Place Already Favorite", StatusCodes.Status409Conflict);

    public static readonly Error PlaceNotFavorite =
        new("Places.PlaceNotFavorite", "Place Not Favorite", StatusCodes.Status409Conflict);
 

}
