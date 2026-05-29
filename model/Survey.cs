using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class Survey
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Feedback { get; set; } = null!;

    [Range(1, 5)]
    public int Rate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
