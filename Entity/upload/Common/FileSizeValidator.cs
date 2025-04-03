
namespace Tourism_Api.Entity.upload.Common;

public class FileSizeValidator : AbstractValidator<IFormFile>
{
    public FileSizeValidator()
    {
        RuleFor(x => x)
            .Must((request, context) => request.Length <= FileSettings.MaxFileSizeInBytes)
            .WithMessage($"Max file size is {FileSettings.MaxFileSizeInMB} MB.")
            .When(x => x is not null);
    }
}