namespace Tourism_Api.Entity.Admin;

public class ContactUsProblemsResponse
{
    public int Count { get; set; }
    public List<ContactUsProblemDto> Problems { get; set; } = [];
}