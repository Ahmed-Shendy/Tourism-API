
using Tourism_Api.Entity.upload.Common;

namespace Tourism_Api.Entity.upload;

public class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
{
    public UploadImageRequestValidator()
    {
        RuleFor(x => x.Image)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignaturesValidator());
            //.SetValidator(new FileNameValidator());

        RuleFor(x => x.Image)
            .Must((request, context) =>
            {
                var extension = Path.GetExtension(request.Image.FileName.ToLower());
                return FileSettings.AllowedImagesExtensions.Contains(extension);
            })
            .WithMessage("File extension is not allowed")
            .When(x => x.Image is not null);
    }
}