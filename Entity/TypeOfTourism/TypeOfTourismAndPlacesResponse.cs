using Tourism_Api.Entity.Places;
using Tourism_Api.model;

namespace Tourism_Api.Entity.TypeOfTourism
{
   // public record TypeOfTourismAndPlacesResponse(string Name , string? Photo , ICollection<PlacesDetails> PlaceNames);

    public class TypeOfTourismAndPlacesResponse
    {
        public string Tourism_Name { get; set; }


        public ICollection<ALLPlaces> PlaceNames { get; set; } = new List<ALLPlaces>();
    }
}
