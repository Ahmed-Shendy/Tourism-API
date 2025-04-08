using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public partial class User : IdentityUser
{
    // public string Id { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Country { get; set; }

    [Column(TypeName = "text")]
    public string? Phone { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Role { get; set; } = "User";

    public string? Gender { get; set; }

    public string? Photo { get; set; }

    public string? TourguidId { get; set; }


    public virtual ICollection<User> InverseTourguid { get; set; } = new List<User>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    public virtual ICollection<PlaceRate> PlaceRates { get; set; } = [];

    public virtual User? Tourguid { get; set; }

    public virtual ICollection<TourguidAndPlaces> TourguidAndPlaces { get; set; }

    public virtual ICollection<FavoritePlace> FavoritePlaces { get; set; } = [];

    // public virtual ICollection<Place> PlaceNames { get; set; } = new List<Place>();
}
