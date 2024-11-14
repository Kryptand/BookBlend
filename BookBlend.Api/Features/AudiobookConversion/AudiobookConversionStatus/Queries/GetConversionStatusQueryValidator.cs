using FluentValidation;

namespace BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus.Queries;

public class GetConversionStatusQueryValidator : AbstractValidator<GetConversionStatusQuery>
{
    public GetConversionStatusQueryValidator()
    {
        RuleFor(x => x.ConversionJobId)
            .NotEmpty();
    }
}