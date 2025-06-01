using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.Entity.Tourguid;


public class Dashboard
{

    public int CountFamle { get; set; } = 0;

    public List<PeopleForCountry>? peopleForCountries { get; set; } = new List<PeopleForCountry>();

    public int CountMale { get; set; } = 0;

    public List<AllTourguids>? allTourguidsByScope { get; set; } = new List<AllTourguids>();


    public List<TopFavoritePlace> topFavoritePlaces { get; set; } = new List<TopFavoritePlace>();
}

public class TopFavoritePlace
{
    public string Name { get; set; } = null!;

    public decimal GoogleRate { get; set; } = 0;

    public string? Photo { get; set; }


}


public class AllNotActiveTourguid
{
    public List<NotActiveTourguids> NotActiveTourguids { get; set; } = new List<NotActiveTourguids>();
    public int Count { get; set; } = 0; 
}

public class NotActiveTourguids
{
    public string Id { get; set; } 
    public string Name { get; set; } 
    public string? Photo { get; set; }
    public string? CV { get; set; }

}


public class AllTourguids
{
    public string Id { get; set; } 

    public string Name { get; set; } = null!;

    
    public string Email { get; set; } = null!;

    public string? Photo { get; set; }



    public DateTime? BirthDate { get; set; } = null!;

    public ulong? countOfTourisms { get; set; } 


    public string? Gender { get; set; }


}

public class PeopleForCountry
{
    public string country { get; set; }
    public int count { get; set; }
}

