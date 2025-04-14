using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Tourguid;

public class TourguidProfile
{
    
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public int? Age { get; set; }

    public ulong? Score { get; set; } 

    public string? Gender { get; set; }

    public string? Photo { get; set; }

    public List<placesinfo> places { get; set; } = null!;
    public List<Tourist>? tourists { get; set; } = new List<Tourist>();

    public int? TouristsCount { get; set; }

}

public class placesinfo
{
    public string Name { get; set; } = null!;
    public string? Photo { get; set; }
}




public class Tourist
{
    public string Id { get; set; }
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }
}