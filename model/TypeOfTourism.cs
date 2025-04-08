using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public partial class TypeOfTourism
{
    public string Name { get; set; } = null!;
    [Column(TypeName = "text")]
    public string? Photo { get; set; }

    public virtual ICollection<Place> PlaceNames { get; set; } = new List<Place>();
}
