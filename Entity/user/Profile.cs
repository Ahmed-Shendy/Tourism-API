using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.user;

public class Profile
{
    
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public string? Photo { get; set; }

    public tourguidinfo? Tourguid { get; set; } = null!;

    public List<FavPlaces> FavoritePlaces { get; set; } = null!;

}

public class  FavPlaces
{
    public string Name { get; set; } 

    public string? Photo { get; set; }

}

public class ProfileUpdate
{

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public string? Photo { get; set; }

}

public class tourguidinfo
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string? Phone { get; set; }
}

