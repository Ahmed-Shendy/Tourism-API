using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Tourguid;


public class Dashboard
{

    public int CountFamle { get; set; } = 0;

    public List<PeopleForCountry>? peopleForCountries { get; set; } = new List<PeopleForCountry>();

    public int CountMale { get; set; } = 0;

    public List<AllTourguids>? allTourguids { get; set; } = new List<AllTourguids>();
}






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

public class PeopleForCountry
{
    public string country { get; set; }
    public int count { get; set; }
}