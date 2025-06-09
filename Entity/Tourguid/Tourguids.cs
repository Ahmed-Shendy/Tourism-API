namespace Tourism_Api.Entity.Tourguid;

public class Tourguids
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public string? Phone { get; set; }

    public bool IsActive { get; set; } = true; // القيمة الافتراضية true

    public bool IsBooked { get; set; } = false; // القيمة الافتراضية false
    public string? Gender { get; set; }

    public decimal? rate { get; set; } = 0;

    public string? Photo { get; set; }
}
