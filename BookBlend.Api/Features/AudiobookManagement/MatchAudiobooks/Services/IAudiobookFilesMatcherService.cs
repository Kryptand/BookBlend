using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public interface IAudiobookFilesMatcherService
{
    Task<IEnumerable<Audiobook?>> MatchAudiobookFilesToAudiobooks(IEnumerable<AudiobookFile> audiobookFiles);
}