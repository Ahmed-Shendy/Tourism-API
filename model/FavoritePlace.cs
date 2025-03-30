using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class FavoritePlace
{

    [ForeignKey("User")]
    public string UserId { get; set; }
    [ForeignKey("Place")]
    public string PlaceName { get; set; }


    public Place Place { get; set; }
    public User User { get; set; }
}
