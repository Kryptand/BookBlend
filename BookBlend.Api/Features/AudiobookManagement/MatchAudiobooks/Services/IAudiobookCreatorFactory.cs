using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public interface IAudiobookCreatorFactory
{
    Audiobook? GetOrCreateAudiobook(
        Dictionary<string, Audiobook?> audiobooks,
        AudiobookFile audiobookFile,
        Chapter chapter);
}