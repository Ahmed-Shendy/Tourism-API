namespace Tourism_Api.Entity.Comment;

public class ReplyToReplyComment
{
    public string Content { get; set; } = null!;
    public int ParentReplyId { get; set; }  // The reply being responded to
}
