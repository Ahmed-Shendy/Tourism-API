using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.user;

namespace Tourism_Api.Entity.Places;

public class PlacesDetails
{
    public string Name { get; set; } = null!;

    public string? Photo { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

   // public decimal? Rate { get; set; }

    public string? GovernmentName { get; set; }

    public List<UserComment>? comments { get; set; } = [];
    public List<Tourguids>? Tourguids { get; set; } = [];
    public List<string>? TypeOfTourism  { get; set; } = [];


}
