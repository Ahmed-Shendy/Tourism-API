using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.Entity.Programs;

public class TripsResponse
{
    public string Name { get; set; } = null!;

    
    public string? Description { get; set; }

    
    public decimal? Price { get; set; }

    public string? Days { get; set; }
}
