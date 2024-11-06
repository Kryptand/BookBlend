using BookBlend.Api.Shared;
using MediatR;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Commands;

public sealed class MatchAudiobookFilesToAudiobooksCommand() : IRequest<Result<MatchAudioobookFilesToAudiobooksCommandResult>>;