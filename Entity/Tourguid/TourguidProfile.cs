using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Tourguid;

public class TourguidProfile
{
    
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? MaxTourists { get; set; }  // الحد الأقصى للسياح

    public bool IsActive { get; set; } = true; // القيمة الافتراضية true

    public int? CurrentTouristsCount { get; set; } = 0; // القيمة الافتراضية 0

    public string Password { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }
    public List<string> AllLangues { get; set; } = [];


    public DateTime? BirthDate { get; set; } = null!;

    public string? CV { get; set; }


    public ulong? Score { get; set; } 

    public string? Gender { get; set; }


    public string? Photo { get; set; }

    public decimal? Rate { get; set; } = 0;


    public List<placesinfo> places { get; set; } = null!;
    public string? TripName { get; set; } = null!;
    public List<Tourist>? tourists { get; set; } = new List<Tourist>();

   // public int? TouristsCount { get; set; }

    public List<RateGroup>? RateGroup { get; set; } = new List<RateGroup>();

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

    public DateTime? BirthDate { get; set; } = null!;

    // public int? Age { get; set; }

    public string? Gender { get; set; }
}