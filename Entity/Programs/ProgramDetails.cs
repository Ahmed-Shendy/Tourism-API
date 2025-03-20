namespace Tourism_Api.Entity.Programs;

public class ProgramDetails
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? Activities { get; set; }

    public List<ProgramsPlaces> programsPlaces { get; set; } = new List<ProgramsPlaces>();
}

public class ProgramsPlaces
{
    public string? Photo { get; set; }
    public string? PlaceName { get; set; }
}
