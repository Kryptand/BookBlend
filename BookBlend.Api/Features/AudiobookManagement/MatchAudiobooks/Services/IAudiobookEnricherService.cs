using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public interface IAudiobookEnricherService
{
    Audiobook EnrichAudiobook(Audiobook audiobook, AudiobookFile file, string chapterName);

    void CalculateAndSetAudiobookDuration(IEnumerable<Audiobook?> audiobooks);
}