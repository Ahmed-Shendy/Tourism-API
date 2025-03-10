using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class PlaceRate
{
    //public int Id { get; set; }


    public decimal Rate { get; set; }


    [ForeignKey("User")]
    public string UserId { get; set; }
    [ForeignKey("Place")]
    public string PlaceName { get; set; }

    
    public Place Place { get; set; }
    public User User { get; set; }
}
