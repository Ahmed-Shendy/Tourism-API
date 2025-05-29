using FluentValidation;

namespace Tourism_Api.Entity.user;

public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordRequest>
{
    private const string Password = "(?=(.*[0-9]))(?=.*[a-z])(?=(.*[A-Z]))";

    public ResetPasswordRequestValidation()
    {

        

        RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Length(8, 100)
            .Matches(Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase , Digites and Uppercase");
    
    }
}
