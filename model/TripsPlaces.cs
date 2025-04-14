using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class TripsPlaces
{
    [ForeignKey("Trip")]
    public string TripName { get; set; } = null!;
    [ForeignKey("Place")]
    public string PlaceName { get; set; } = null!;

    public virtual Trips Trip { get; set; } = null!;
    public virtual Place Place { get; set; } = null!;
}
