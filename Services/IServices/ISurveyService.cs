using Tourism_Api.Entity.Survey;

namespace Tourism_Api.Services.IServices;

public interface ISurveyService
{
    Task<Result> AddSurvey(string userId, AddSurveyRequest request, CancellationToken cancellationToken = default);
    Task<Result<List<SurveyResponse>>> GetMySurveys(string userId, CancellationToken cancellationToken = default);
    Task<Result<List<SurveyResponse>>> GetAllSurveys(CancellationToken cancellationToken = default);
    Task<Result> DeleteSurvey(int surveyId, string userId, bool isAdmin, CancellationToken cancellationToken = default);
}
