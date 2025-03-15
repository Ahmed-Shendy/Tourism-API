using Tourism_Api.Entity.Places;

namespace Tourism_Api.Entity.Governorate
{
    
     public record GovernorateAndPLacesResponse(string Name, string? Photo, List<PlacesDetails> Places);

}
