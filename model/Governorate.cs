using System;
using System.Collections.Generic;

namespace Tourism_Api.model;

public partial class Governorate
{
    public string Name { get; set; } = null!;

    public string? Photo { get; set; }

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
