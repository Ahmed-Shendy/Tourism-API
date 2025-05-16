using Tourism_Api.Entity.Places;

namespace Tourism_Api.Entity.Governorate
{
    
     public record GovernorateAndPLacesResponse(string Name, string? Photo, List<PlacesDetails> Places);

}
public class GovernorateALLPlaces
{
   // private static readonly IQueryable<ALLPlaces> aLLPlaces = new IQueryable<ALLPlaces>();

    public string Governorate_Name { get; set; }

    public string Name { get; set; }

    public string? Photo { get; set; }

    public decimal GoogleRate { get; set; }

    // public IQueryable<ALLPlaces> Places { get; set; } = new List<ALLPlaces>().AsQueryable();


}

public class ALLPGeneratorResponse
{
    public List<ALLPGenerator> AllGovernorate { get; set; } = [];
}
public class ALLPGenerator
{
    public string Name { get; set; }
    public string? Photo { get; set; }
}

