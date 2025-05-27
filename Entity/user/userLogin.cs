using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.user;

public class userLogin
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Password must be at least 3 to 50 characters long")]
    public string Password { get; set; }

}
