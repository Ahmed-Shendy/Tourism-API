using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Tourguid;

public class AddTourguidRequest
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public DateOnly? BirthDate { get; set; }

    //public string? Role { get; set; } = "User";

    public string? Gender { get; set; }

    public string? Photo { get; set; }

   // public string? TourguidId { get; set; }
}
