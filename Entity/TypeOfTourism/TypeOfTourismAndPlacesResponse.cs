using Tourism_Api.Entity.Places;
using Tourism_Api.model;

namespace Tourism_Api.Entity.TypeOfTourism
{
   // public record TypeOfTourismAndPlacesResponse(string Name , string? Photo , ICollection<PlacesDetails> PlaceNames);

    public class TypeOfTourismAndPlacesResponse
    {
       public string Tourism_Name { get; set; }

       public List<ALLPlaces> ALLPlaces { get; set; } = new List<ALLPlaces>();

        //public IQueryable<TypeOfTourismALLPlaces> PlaceNames { get ; set ; } = new List<TypeOfTourismALLPlaces>().AsQueryable();
    }
}

public class TypeOfTourismALLPlaces
{
    public string Tourism_Name { get; set; }

    public string Name { get; set; }

    public string? Photo { get; set; }

    public decimal GoogleRate { get; set; }

    

}
