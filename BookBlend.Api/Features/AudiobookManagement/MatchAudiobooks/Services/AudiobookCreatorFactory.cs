using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public class AudiobookCreatorFactory(
    IAudiobookFileMapper audiobookFileMapper,
    AudiobookTitleHelper audiobookTitleHelper)
    : IAudiobookCreatorFactory
{
    public Audiobook? GetOrCreateAudiobook(
        Dictionary<string, Audiobook?> audiobooks,
        AudiobookFile audiobookFile,
        Chapter chapter)
    {
        var audiobookTitle = audiobookTitleHelper.DetermineAudiobookTitle(audiobookFile, chapter.Title);

        if (!audiobooks.TryGetValue(audiobookTitle, out var audiobook))
        {
            audiobook = audiobookFileMapper.MapAudiobookFileToAudiobook(audiobookFile, chapter.Title, "de");
            audiobook.Title = audiobookTitle;
            audiobooks[audiobookTitle] = audiobook;
        }

        return audiobook;
    }
}