namespace Tourism_Api.Entity.Comment;

public class ReplyComment
{
    public string Content { get; set; } = null!;
    public int ParentCommentId { get; set; }
    public string PlaceName { get; set; } = null!;
}
