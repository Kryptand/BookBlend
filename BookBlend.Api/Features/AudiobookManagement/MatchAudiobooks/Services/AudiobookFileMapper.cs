using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public sealed class AudiobookFileMapper(FileNameHelper fileNameHelper) : IAudiobookFileMapper
{
    public Audiobook MapAudiobookFileToAudiobook(AudiobookFile audiobookFile, string chapterName,
        string defaultLanguage)
    {
        ArgumentNullException.ThrowIfNull(audiobookFile);
        ArgumentNullException.ThrowIfNull(chapterName);
        ArgumentNullException.ThrowIfNull(defaultLanguage);
        
        var durationStringToTimeSpan = TimeSpan.Parse(audiobookFile.Duration);
        
        var dateTime = DateTime.Parse(audiobookFile.Metadata?.ReleaseDate ?? DateTime.Now.ToString("yyyy-MM-dd" ));
        
        var audiobook = new Audiobook
        {
            Title = chapterName,
            Duration = durationStringToTimeSpan,
            Authors = audiobookFile.Metadata?.Authors?.ToList() ?? new List<string>(),
            Narrators = audiobookFile.Metadata?.Narrators?.ToList() ?? new List<string>(),
            Publisher = audiobookFile.Metadata?.Publisher,
            ReleaseDate = dateTime,
            Language = defaultLanguage,
            Description = audiobookFile.Metadata?.Description,
            CoverImage = audiobookFile.Metadata?.Images.FirstOrDefault()
        };

        return audiobook;
    }

    public Chapter MapAudiobookFileToChapter(AudiobookFile audiobookFile)
    {
        ArgumentNullException.ThrowIfNull(audiobookFile);

        var durationStringToTimeSpan = TimeSpan.Parse(audiobookFile.Duration);

        var chapter = new Chapter
        {
            Title = fileNameHelper.GetFileNameWithoutExtension(audiobookFile.FileName),
            Duration = durationStringToTimeSpan,
            AudioFile = audiobookFile,
            TrackNumber = audiobookFile.Metadata?.Track ?? 0
        };

        return chapter;
    }
}