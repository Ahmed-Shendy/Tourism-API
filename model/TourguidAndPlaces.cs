using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class TourguidAndPlaces
{
    [ForeignKey("User")]
    public string TouguidId { get; set; }
    [ForeignKey("Place")]
    public string PlaceName { get; set; }


    public Place Place { get; set; }
    public User Touguid { get; set; }
}
