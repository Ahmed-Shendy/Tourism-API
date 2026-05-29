namespace Tourism_Api.Entity.user;

public class EmailConfirmationViewModel
{
    public bool IsSuccess { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string ProjectName { get; set; } = "Egypt_Guid";
    public string Icon { get; set; } = "check";
    public string? ActionUrl { get; set; }
    public string? ActionText { get; set; }
}
