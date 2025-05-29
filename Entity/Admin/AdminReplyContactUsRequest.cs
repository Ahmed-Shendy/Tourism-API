namespace Tourism_Api.Entity.Admin;

public class AdminReplyContactUsRequest
{
    public int ContactUsId { get; set; }
    public string ReplyMessage { get; set; } = null!;
}