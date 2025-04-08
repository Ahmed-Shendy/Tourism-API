using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class TourguidAndPlaces
{
    [ForeignKey("Touguid")]
    public string TouguidId { get; set; }
    
    [ForeignKey("Place")]
    public string PlaceName { get; set; }

    public string? MoveToPlace { get; set; } = null!;

    public User Touguid { get; set; }
    public Place Place { get; set; }
}
