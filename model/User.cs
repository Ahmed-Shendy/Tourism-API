using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public string? Phone { get; set; }

    public string? language { get; set; } = null;

    public DateTime? BirthDate { get; set; } = null;

    public ulong? Score { get; set; } = 0 ;

    public string? Role { get; set; } = "User";

    public string? Gender { get; set; }
    
    public string? ConfirmCode { get; set; }


    [Column(TypeName = "text")]
    public string? Photo { get; set; }

    public string? CV { get; set; }
    public string?  CvContentType { get; set; }
    public string? StoredFileName { get; set; }


    public string? ContentType { get; set; }
     [Column(TypeName = "bit")]
     [DefaultValue(true)]
    public bool IsActive { get; set; } = true; // القيمة الافتراضية true
    
    public int? MaxTourists { get; set; }  // الحد الأقصى للسياح
    [Column(TypeName = "int")]
    [DefaultValue(0)]
    public int? CurrentTouristsCount { get; set; } = 0; // القيمة الافتراضية 0

    public string? TourguidId { get; set; }

    [ForeignKey("Program")]
    public string? ProgramName { get; set; }

    public Program? Program { get; set; }

    [ForeignKey("Trip")]
    public string? TripName { get; set; }
    //[ForeignKey("Trip")]
    public Trips? Trip { get; set; }

    public string? MoveToTrip { get; set; }

    public virtual ICollection<User> InverseTourguid { get; set; } = new List<User>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    public virtual ICollection<PlaceRate> PlaceRates { get; set; } = [];

    public virtual User? Tourguid { get; set; }

    public virtual ICollection<TourguidAndPlaces> TourguidAndPlaces { get; set; }

    //public virtual ICollection<Tourguid_Rate> UserTourguid_Rates { get; set; }

    public virtual ICollection<Tourguid_Rate> Tourguid_Rates { get; set; }

    public virtual ICollection<FavoritePlace> FavoritePlaces { get; set; } = [];

    public virtual ICollection<Langues> Langues { get; set; } = [];


    // public virtual ICollection<Place> PlaceNames { get; set; } = new List<Place>();
}
