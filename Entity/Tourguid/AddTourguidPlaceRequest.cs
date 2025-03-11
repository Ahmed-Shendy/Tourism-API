using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Tourguid;

public class AddTourguidPlaceRequest
{
    [Required]
    public string TourguidId { get; set; }
    [Required]
    public string PlaceName { get; set; } 
}
