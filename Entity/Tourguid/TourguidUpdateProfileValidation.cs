namespace Tourism_Api.Entity.user;

public class TourguidUpdateProfileValidation : AbstractValidator<TourguidUpdateProfile>
{
    private const string Password = "(?=(.*[0-9]))(?=.*[a-z])(?=(.*[A-Z]))";

    public TourguidUpdateProfileValidation()
    {



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
