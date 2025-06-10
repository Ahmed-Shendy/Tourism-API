using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tourism_Api.Entity.Tourguid;

namespace Tourism_Api.Entity.Programs;

public class TripDetails
{
   
    public string Name { get; set; } = null!;

    
    public string? Description { get; set; }

    
    public decimal? Price { get; set; }

    public int? Days { get; set; }
    public int? Number_of_Sites { get; set; }


    public string programName { get; set; }

    public List<Trip_Places> TripPlaces { get; set; } = new List<Trip_Places>();

    public List<Tourguids>? Tourguids { get; set; } = [];
}

public class Trip_Places
{
    public string? Photo { get; set; }
    public string? Name { get; set; }

}
