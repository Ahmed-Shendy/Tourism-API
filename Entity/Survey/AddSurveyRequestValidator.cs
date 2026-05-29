namespace Tourism_Api.Entity.Survey;

public class AddSurveyRequestValidator : AbstractValidator<AddSurveyRequest>
{
    public AddSurveyRequestValidator()
    {
        RuleFor(x => x.Feedback)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.Rate)
            .InclusiveBetween(1, 5);
    }
}
