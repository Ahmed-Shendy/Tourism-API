using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Tourguid;

public class TourguidUpdateProfile
{
    
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

   


}
