using FluentValidation;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Commands;

public sealed class ConvertAndMergeAudiobooksToM4aCommandValidator : AbstractValidator<ConvertAndMergeAudiobooksToM4aCommand>
{
    public ConvertAndMergeAudiobooksToM4aCommandValidator()
    {
        RuleFor(x => x.ConversionJobId)
            .NotEmpty();
        RuleFor(x => x.ConversionJobId)
            .NotEqual(Guid.Empty);
    }
}