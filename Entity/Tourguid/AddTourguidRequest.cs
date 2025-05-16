using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Tourguid;

public class AddTourguidRequest
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    
    public string Country { get; set; }

    public string Phone { get; set; }

    public DateTime? BirthDate { get; set; } = null!;

    public string Gender { get; set; }

    public string? TripName { get; set; }

    public string? PlaceName { get; set; }

    public List<string> AllLangues { get; set; } = new List<string>();

    public required IFormFile Cvfile { get; set; }
   

    public required IFormFile image { get; set; }


}
