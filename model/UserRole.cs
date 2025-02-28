using Microsoft.AspNetCore.Identity;

namespace Tourism_Api.model;

public class UserRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}
