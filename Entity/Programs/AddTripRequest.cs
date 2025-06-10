using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.Entity.Programs;

public class AddTripRequest
{
    public string Name { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Price { get; set; }

    public int? Days { get; set; }

    public string programName { get; set; }

    public virtual ICollection<string> TripsPlaces { get; set; } = [];
}
