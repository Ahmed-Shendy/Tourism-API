namespace Tourism_Api.Entity.Places;

public class AddPlaceRequest
{
    public string Name { get; set; } = null!;

    public string? Photo { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public string? VisitingHours { get; set; }
    
    public decimal GoogleRate { get; set; } = 0;

    public string GovernmentName { get; set; }

    public List<string> TypeOfTourism { get; set; } = [];

}

public class UpdatePlaceRequest
{
    //public string Name { get; set; } = null!;

    public string? Photo { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public string? VisitingHours { get; set; }

    public decimal GoogleRate { get; set; } = 0;

   // public string GovernmentName { get; set; }

}