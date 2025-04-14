using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tourism_Api.Entity.Tourguid;

namespace Tourism_Api.Entity.Programs;

public class TripDetails
{
   
    public string Name { get; set; } = null!;

    
    public string? Description { get; set; }

    
    public decimal? Price { get; set; }

    public string? Days { get; set; }

   
    public string programName { get; set; }

    public List<TripPlaces> TripPlaces { get; set; } = new List<TripPlaces>();

    public List<Tourguids>? Tourguids { get; set; } = [];
}

public class TripPlaces
{
    public string? Photo { get; set; }
    public string? Name { get; set; }
}
