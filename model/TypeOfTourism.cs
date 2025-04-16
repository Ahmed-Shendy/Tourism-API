using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public partial class TypeOfTourism
{
    public string Name { get; set; } = null!;
    [Column(TypeName = "text")]
    public string? Photo { get; set; }

    public virtual ICollection<Type_of_Tourism_Places> Type_Of_Tourism_Places { get; set; } = [];
}
