using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.user;

public class Profile
{
    
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public DateTime? BirthDate { get; set; } = null!;

    public string? language { get; set; } = null;

    public string? Gender { get; set; }

    public string? Photo { get; set; }

    public tourguidinfo? Tourguid { get; set; } = null!;

    public List<FavPlaces> FavoritePlaces { get; set; } = null!;

}

public class Public_Profile
{

    public string Name { get; set; } = null!;

    public string? Country { get; set; }

    public string? language { get; set; } = null;

    public tourguidinfo? Tourguid { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime? BirthDate { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Photo { get; set; }

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


    public string? Phone { get; set; }


}

public class tourguidinfo
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string? Photo { get; set; }
    public decimal Rate { get; set; }
}

