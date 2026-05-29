using Tourism_Api.Abstractions;

namespace Tourism_Api.Errors;

public static class SurveyErrors
{
    public static readonly Error SurveyNotFound =
        new("Survey.NotFound", "Survey was not found.", StatusCodes.Status404NotFound);

    public static readonly Error UserNotAllowed =
        new("Survey.UserNotAllowed", "You are not allowed to delete this survey.", StatusCodes.Status403Forbidden);
}
