using BookBlend.Api.Shared;
using MediatR;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Commands;

public sealed class ConvertAndMergeAudiobooksToM4aCommand() : IRequest<Result>
{
    public Guid ConversionJobId { get; set; }
}