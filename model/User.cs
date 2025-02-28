using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Tourism_Api.model;

public partial class User : IdentityUser
{
   // public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Role { get; set; }

    public string? Gender { get; set; }

    public string? Photo { get; set; }

    public string? TourguidId { get; set; }

    public virtual ICollection<User> InverseTourguid { get; set; } = new List<User>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    public virtual User? Tourguid { get; set; }

    public virtual ICollection<Place> PlaceNames { get; set; } = new List<Place>();
}
