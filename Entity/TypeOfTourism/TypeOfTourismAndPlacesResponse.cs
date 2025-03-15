using Tourism_Api.Entity.Places;
using Tourism_Api.model;

namespace Tourism_Api.Entity.TypeOfTourism
{
    public record TypeOfTourismAndPlacesResponse(string Name , string? Photo , ICollection<PlacesDetails> PlaceNames);
}
