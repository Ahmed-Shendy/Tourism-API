using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Tourguid;

public class AllTourguids
{
    public string Id { get; set; } 

    public string Name { get; set; } = null!;

    
    public string Email { get; set; } = null!;

   


    public string? Phone { get; set; }

    public int? Age { get; set; }

    public ulong? countOfTourisms { get; set; } 

    public int PlaceCount { get; set; }

    public string? Gender { get; set; }

    

    public List<string> PlaceNames { get; set; } = [];

}
