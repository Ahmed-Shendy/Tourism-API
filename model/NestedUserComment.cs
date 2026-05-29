using System;
using System.Collections.Generic;

namespace Tourism_Api.model;

public class NestedUserComment
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? Photo { get; set; }
    public string UserId { get; set; } = null!;
    public int LikesCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ParentReplyId { get; set; }

    // Recursive property for nested replies
    public List<NestedUserComment> Replies { get; set; } = [];
}
