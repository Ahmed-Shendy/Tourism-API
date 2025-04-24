using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class Trips
{
    [Key]
    public string Name { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Price { get; set; }

    public int? Days { get; set; }

    public int? Number_of_Sites{ get; set; }

    [ForeignKey("Program")]
    public string programName { get; set; }


    public Program Program { get; set; }

    //public virtual ICollection<ProgramsPhoto> ProgramsPhotos { get; set; } = new List<ProgramsPhoto>();
    public virtual ICollection<TripsPlaces> TripsPlaces { get; set; } = [];

    public virtual ICollection<User> Tourguids { get; set; } = new List<User>();
}
