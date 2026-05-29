using Microsoft.EntityFrameworkCore;
using Tourism_Api.Entity.Survey;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

public class SurveyService(TourismContext db) : ISurveyService
{
    private readonly TourismContext db = db;

    public async Task<Result> AddSurvey(string userId, AddSurveyRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await db.Users.AnyAsync(x => x.Id == userId, cancellationToken);
        if (!userExists)
            return Result.Failure(UserErrors.UserNotFound);

        var survey = new Survey
        {
            UserId = userId,
            Feedback = request.Feedback,
            Rate = request.Rate
        };

        await db.Surveys.AddAsync(survey, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<List<SurveyResponse>>> GetMySurveys(string userId, CancellationToken cancellationToken = default)
    {
        var surveys = await db.Surveys
            .Include(x => x.User)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SurveyResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.User.Name,
                Feedback = x.Feedback,
                Rate = x.Rate,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(surveys);
    }

    public async Task<Result<List<SurveyResponse>>> GetAllSurveys(CancellationToken cancellationToken = default)
    {
        var surveys = await db.Surveys
            .Include(x => x.User)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SurveyResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.User.Name,
                Feedback = x.Feedback,
                Rate = x.Rate,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(surveys);
    }

    public async Task<Result> DeleteSurvey(int surveyId, string userId, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var survey = await db.Surveys.SingleOrDefaultAsync(x => x.Id == surveyId, cancellationToken);
        if (survey is null)
            return Result.Failure(SurveyErrors.SurveyNotFound);

        if (!isAdmin && survey.UserId != userId)
            return Result.Failure(SurveyErrors.UserNotAllowed);

        db.Surveys.Remove(survey);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
