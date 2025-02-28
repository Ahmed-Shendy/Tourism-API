using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.user;

public class userLogin
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }

}
