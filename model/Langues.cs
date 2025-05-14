using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class Langues
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }

    public string Langue { set; get; }
    [ForeignKey("User")]
    public string TourguidId { get; set; }

    public User User { get; set; }
}
