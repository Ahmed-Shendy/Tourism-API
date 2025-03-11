using System;
using System.Collections.Generic;

namespace Tourism_Api.model;

public partial class Place
{
    public string Name { get; set; } = null!;

    public string? Photo { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public decimal? Rate { get; set; }

    public string? GovernmentName { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Governorate? GovernmentNameNavigation { get; set; }

    public virtual ICollection<Program> ProgramNames { get; set; } = new List<Program>();

    public virtual ICollection<User>? Tourguids { get; set; } = new List<User>();
    
    public virtual ICollection<TourguidAndPlaces> TourguidAndPlaces { get; set; }


    public virtual ICollection<TypeOfTourism> TourismNames { get; set; } = new List<TypeOfTourism>();

    public virtual ICollection<PlaceRate> PlaceRates { get; set; } = [];

}
