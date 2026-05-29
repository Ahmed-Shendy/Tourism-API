using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class CommentReplyLike
{
    [ForeignKey("CommentReply")]
    public int ReplyId { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual CommentReply CommentReply { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
