using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class CommentLike
{
    [ForeignKey("Comment")]
    public int CommentId { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Comment Comment { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
