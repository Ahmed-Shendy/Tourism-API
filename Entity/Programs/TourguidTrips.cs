using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Programs;

public class TourguidTrips
{
    [Required]
    public string TourguidId { get; set; }
    [Required]
    public string MoveTo { get; set; }
    
}
