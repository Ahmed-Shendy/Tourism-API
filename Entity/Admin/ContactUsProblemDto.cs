namespace Tourism_Api.Entity.Admin;

public class ContactUsProblemDto
{
    public int Id { get; set; }
    public string Problem { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string? UserPhoto { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsResolved { get; set; }
}
