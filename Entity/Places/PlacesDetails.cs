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
    public List<UswerRate>? UserRates { get; set; } = [];


}

public class UswerRate
{
    public string? UserId { get; set; } = null!;
    public string? UserName { get; set; } = null!;
    public string? photo { get; set; } = null!;
    public decimal Rate { get; set; } = 0;
}
