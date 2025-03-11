namespace Tourism_Api.Entity.Places;

public class AddPlaceRequest
{
    public string Name { get; set; } = null!;

    public string? Photo { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

  //  public decimal? Rate { get; set; }

  //  public string? GovernmentName { get; set; }

}
