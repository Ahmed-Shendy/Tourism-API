using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class Tourguid_Rate
{
    [ForeignKey(nameof(Tourguid))]
    public required string tourguidId { get; set; }
    [ForeignKey(nameof(User))]
    public required string userId { get; set; }

    [Range(1 , 5)]
    public int rate { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual User Tourguid { get; set; } = null!;
}
