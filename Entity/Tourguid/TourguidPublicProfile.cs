namespace Tourism_Api.Entity.Tourguid;

public class TourguidPublicProfile
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public DateTime? BirthDate { get; set; } = null!;
    public List<string> AllLangues { get; set; } = [];


    // public int? Age { get; set; }
    public bool IsActive { get; set; } = true; // القيمة الافتراضية true


    public ulong? Score { get; set; } 

    public string? Gender { get; set; }

    public string? Photo { get; set; }

    public decimal? Rate { get; set; } = 0;

    public List<placesinfo> places { get; set; } = null!;

    public string? TripName { get; set; } = null!;

    public List<Tourist>? tourists { get; set; } = new List<Tourist>();

    public int? TouristsCount { get; set; }
    public List<RateGroup>? RateGroup { get; set; } = new List<RateGroup>();


}

public class RateGroup
{
    //public int? count_5_Rate   { get; set; } = 0;
    //public int? count_4_Rate { get; set; } = 0;
    //public int? count_3_Rate { get; set; } = 0;
    //public int? count_2_Rate { get; set; } = 0;
    //public int? count_1_Rate { get; set; } = 0;
    public int? value { get; set; } = 0;
    public int count { get; set; } = 0;



}
