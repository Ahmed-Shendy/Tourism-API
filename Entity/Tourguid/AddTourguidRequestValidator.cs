using Tourism_Api.Entity.upload.Common;

namespace Tourism_Api.Entity.Tourguid;

public class AddTourguidRequestValidator : AbstractValidator<AddTourguidRequest>
{
    public AddTourguidRequestValidator()
    {
        RuleFor(x => x.image)
           .SetValidator(new FileSizeValidator())
           .SetValidator(new BlockedSignaturesValidator())
           . When(x => x.image is not null);
        //.SetValidator(new FileNameValidator());

        RuleFor(x => x.image)
            .Must((request, context) =>
            {
                var extension = Path.GetExtension(request.image.FileName.ToLower());
                return FileSettings.AllowedImagesExtensions.Contains(extension);
            })
            .WithMessage("File extension is not allowed")
            .When(x => x.image is not null);

        RuleFor(x => x.Cvfile)
           .SetValidator(new FileSizeValidator())
           .SetValidator(new BlockedSignaturesValidator())
          // .SetValidator(new FileNameValidator())
           .When(x => x.Cvfile is not null);

    }
}
