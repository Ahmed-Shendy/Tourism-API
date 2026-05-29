using Tourism_Api.model;

namespace Tourism_Api.Entity.user;

public class UserRespones
{

    public string Name { get; set; } = null!;
    public string Id { get; set; }
    public string Role { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiretion { get; set; }
    public string? Message { get; set; }

}
