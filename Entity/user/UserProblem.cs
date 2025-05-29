using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.user;

public class UserProblem
{
    [Required]
    public string Problem { get; set; } = string.Empty;
}
