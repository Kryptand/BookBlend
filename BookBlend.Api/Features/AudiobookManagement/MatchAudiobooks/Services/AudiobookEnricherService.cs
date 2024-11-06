using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public sealed class AudiobookEnricherService(IAudiobookFileMapper audiobookFileMapper) : IAudiobookEnricherService
{
    public Audiobook EnrichAudiobook(Audiobook audiobook, AudiobookFile file, string chapterName)
    {
        var mappedAudiobook = audiobookFileMapper.MapAudiobookFileToAudiobook(file, chapterName, "de");

        audiobook.Title ??= mappedAudiobook.Title;

        audiobook.Authors = audiobook.Authors.Count > mappedAudiobook.Authors.Count
            ? audiobook.Authors
            : mappedAudiobook.Authors;

        audiobook.Narrators = audiobook.Narrators.Count > mappedAudiobook.Narrators.Count
            ? audiobook.Narrators
            : mappedAudiobook.Narrators;

        audiobook.Publisher ??= mappedAudiobook.Publisher;
        audiobook.ReleaseDate ??= mappedAudiobook.ReleaseDate;
        audiobook.Description ??= mappedAudiobook.Description;
        audiobook.CoverImage ??= mappedAudiobook.CoverImage;

        return audiobook;
    }

    public void CalculateAndSetAudiobookDuration(IEnumerable<Audiobook?> audiobooks)
    {
        foreach (var audiobook in audiobooks)
        {
            if (audiobook == null)
            {
                throw new InvalidOperationException("Audiobook should not be null.");
            }

            audiobook.TotalTracks = audiobook.Chapters.Count;
            audiobook.Duration = CalculateTotalDuration(audiobook.Chapters);
        }
    }

    private static TimeSpan CalculateTotalDuration(IEnumerable<Chapter> chapters)
    {
        var totalDurationSeconds = chapters.Sum(c =>
            c.Duration.TotalSeconds);
        
        return TimeSpan.FromSeconds(totalDurationSeconds);
    }
}