using System;
using System.Collections.Generic;

namespace Tourism_Api.model;

public partial class Program
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? Activities { get; set; }

    public virtual ICollection<ProgramsPhoto> ProgramsPhotos { get; set; } = new List<ProgramsPhoto>();

    public virtual ICollection<Place> PlaceNames { get; set; } = new List<Place>();
}
