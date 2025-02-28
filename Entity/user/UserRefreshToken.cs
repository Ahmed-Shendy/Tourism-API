using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.user;

public class UserRefreshToken
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string RefrehToken { get; set; }

}
