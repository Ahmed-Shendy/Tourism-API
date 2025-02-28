using FluentValidation;

namespace Tourism_Api.Entity.user;

public class UserRequestValidation : AbstractValidator<UserRequest>
{
    private const string Password = "(?=(.*[0-9]))(?=.*[a-z])(?=(.*[A-Z]))";

    public UserRequestValidation()
    {

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(8, 100)
            .Matches(Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase , Digites and Uppercase");
    
    }
}
