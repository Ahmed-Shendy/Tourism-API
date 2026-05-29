using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public class CommentReply
{
    public int Id { get; set; }

    [Column(TypeName = "text")]
    public string Content { get; set; } = null!;

    [ForeignKey("Comment")]
    public int CommentId { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = null!;

    // Self-referential foreign key for parent reply (nullable - allows top-level replies)
    [ForeignKey("ParentReply")]
    public int? ParentReplyId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Existing relationships
    public virtual Comment Comment { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<CommentReplyLike> Likes { get; set; } = [];

    // Self-referential relationships for nested replies
    public virtual CommentReply? ParentReply { get; set; }
    public virtual ICollection<CommentReply> ChildReplies { get; set; } = [];
}
