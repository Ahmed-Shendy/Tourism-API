using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.Entity.Programs;

public class TripsResponse
{
    public string Name { get; set; } = null!;

    
    public string? Description { get; set; }

    public int? Number_of_Sites { get; set; }


    public decimal? Price { get; set; }

    public int? Days { get; set; }
}
