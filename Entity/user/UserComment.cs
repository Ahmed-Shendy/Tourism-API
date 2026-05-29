namespace Tourism_Api.Entity.user;

public class UserComment
{
    public string Content { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set;  }
    public string? photo { get; set; }
    public int id { get; set; }
    public int LikesCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<UserComment>? Replies { get; set; }
}

