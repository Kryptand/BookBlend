using BookBlend.Api.Shared;
using MediatR;

namespace BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus.Commands;

public sealed class RegisterAudiobookConversionCommand: IRequest<Result<Guid>>
{
    public Guid[] AudiobookIds { get; set; } = Array.Empty<Guid>();
}