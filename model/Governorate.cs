using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public partial class Governorate
{
    public string Name { get; set; } = null!;
    [Column(TypeName = "text")]
    public string? Photo { get; set; }

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
