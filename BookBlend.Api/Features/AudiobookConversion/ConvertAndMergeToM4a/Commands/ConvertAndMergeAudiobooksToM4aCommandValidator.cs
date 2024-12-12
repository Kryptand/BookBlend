using FluentValidation;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Commands;

public sealed class ConvertAndMergeAudiobooksToM4ACommandValidator : AbstractValidator<ConvertAndMergeAudiobooksToM4ACommand>
{
    public ConvertAndMergeAudiobooksToM4ACommandValidator()
    {
        RuleFor(x => x.ConversionJobId)
            .NotEmpty();
        RuleFor(x => x.ConversionJobId)
            .NotEqual(Guid.Empty);
    }
}