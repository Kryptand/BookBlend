using BookBlend.Api.Database;
using BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;
using BookBlend.Api.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Commands;

public sealed class MatchAudiobookFilesToAudiobooksCommandHandler(
    IAudiobookFilesMatcherService audiobookFilesMatcherService,
    AudiobookDbContext context,
    ILogger<MatchAudiobookFilesToAudiobooksCommandHandler> logger)
    : IRequestHandler<
        MatchAudiobookFilesToAudiobooksCommand, Result<MatchAudioobookFilesToAudiobooksCommandResult>>
{
    public async Task<Result<MatchAudioobookFilesToAudiobooksCommandResult>> Handle(
        MatchAudiobookFilesToAudiobooksCommand request, CancellationToken cancellationToken)
    {

        var persistedAudiobooks = await context.AudiobookFiles.ToListAsync(cancellationToken);

        var matchedAudiobooks =
            await audiobookFilesMatcherService.MatchAudiobookFilesToAudiobooks(persistedAudiobooks);

        context.Audiobooks.AddRange(matchedAudiobooks);

        await context.SaveChangesAsync(cancellationToken);

        await Task.CompletedTask;

        return Result.Success(new MatchAudioobookFilesToAudiobooksCommandResult
            { MatchedAudiobooks = matchedAudiobooks.Count() });
    }
}