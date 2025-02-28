using System;
using System.Collections.Generic;

namespace Tourism_Api.model;

public partial class TypeOfTourism
{
    public string Name { get; set; } = null!;

    public string? Photo { get; set; }

    public virtual ICollection<Place> PlaceNames { get; set; } = new List<Place>();
}
