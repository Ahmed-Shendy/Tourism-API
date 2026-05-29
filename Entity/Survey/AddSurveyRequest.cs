namespace Tourism_Api.Entity.Survey;

public class AddSurveyRequest
{
    public string Feedback { get; set; } = string.Empty;
    public int Rate { get; set; }
}
