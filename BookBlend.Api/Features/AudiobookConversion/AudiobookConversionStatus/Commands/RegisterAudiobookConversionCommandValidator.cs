using FluentValidation;

namespace BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus.Commands;

public sealed class RegisterAudiobookConversionCommandValidator : AbstractValidator<RegisterAudiobookConversionCommand>
{
    public RegisterAudiobookConversionCommandValidator()
    {
        RuleFor(x => x.AudiobookIds)
            .NotEmpty();
        RuleForEach(x => x.AudiobookIds)
            .NotEmpty();
    }
}