namespace Tourism_Api.Entity.Survey;

public class SurveyResponse
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Feedback { get; set; } = string.Empty;
    public int Rate { get; set; }
    public DateTime CreatedAt { get; set; }
}
