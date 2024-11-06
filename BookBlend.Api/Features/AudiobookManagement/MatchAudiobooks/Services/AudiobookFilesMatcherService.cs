using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public sealed class AudiobookFilesMatcherService(
    IAudiobookFileMapper audiobookFileMapper,
    IAudiobookEnricherService audiobookEnricher,
    IAudiobookCreatorFactory audiobookCreatorFactory)
    : IAudiobookFilesMatcherService
{
    public async Task<IEnumerable<Audiobook?>> MatchAudiobookFilesToAudiobooks(
        IEnumerable<AudiobookFile> audiobookFiles)
    {
        var audiobooks = new Dictionary<string, Audiobook?>();

        foreach (var audiobookFile in audiobookFiles) await ProcessAudiobookFile(audiobookFile, audiobooks);

        audiobookEnricher.CalculateAndSetAudiobookDuration(audiobooks.Values);

        return audiobooks.Values.AsEnumerable();
    }

    private async Task ProcessAudiobookFile(AudiobookFile audiobookFile, Dictionary<string, Audiobook?> audiobooks)
    {
        var chapter = audiobookFileMapper.MapAudiobookFileToChapter(audiobookFile);
        var audiobook = audiobookCreatorFactory.GetOrCreateAudiobook(audiobooks, audiobookFile, chapter);

        audiobookEnricher.EnrichAudiobook(audiobook, audiobookFile, chapter.Title);
        audiobook.Chapters.Add(chapter);

        await Task.CompletedTask;
    }
}