namespace Tourism_Api.Entity.Tourguid;

public class TourguidPublicProfile
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public string? Photo { get; set; }

    public string placeName { get; set; } = null!;
    public List<Tourist>? tourists { get; set; } = new List<Tourist>();

    public int? TouristsCount { get; set; }
}
