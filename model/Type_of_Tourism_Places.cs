using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class Type_of_Tourism_Places
{

    [ForeignKey("Tourism")]
    public string Tourism_Name { get; set; }

    [ForeignKey("Place")]
    public string Place_Name { get; set; }

    public virtual Place Place { get; set; } = null!;
    public virtual TypeOfTourism Tourism { get; set; } = null!;
}
