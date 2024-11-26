using System.Globalization;
using System.Text.RegularExpressions;
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
        
        TryParseFlexible(audiobookFile.Metadata?.ReleaseDate ?? DateTime.Now.ToString("yyyy-MM-dd"), out var dateTime);
        
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

  

    static bool TryParseFlexible(string dateString, out DateTime result)
    {
        result = default;

        if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            return true;
        
        if (DateTime.TryParseExact(dateString, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            result = new DateTime(result.Year, result.Month, 1);
            return true;
        }

        if (DateTime.TryParseExact(dateString, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            result = new DateTime(result.Year, 1, 1);
            return true;
        }

        return false;
}

    public Chapter MapAudiobookFileToChapter(AudiobookFile audiobookFile)
    {
        ArgumentNullException.ThrowIfNull(audiobookFile);

        var durationStringToTimeSpan = TimeSpan.Parse(audiobookFile.Duration);

        
        var trackNumber= FindTrackNumber(audiobookFile);
        
        var chapter = new Chapter
        {
            Title = fileNameHelper.GetFileNameWithoutExtension(audiobookFile.FileName),
            Duration = durationStringToTimeSpan,
            AudioFile = audiobookFile,
            TrackNumber = trackNumber
        };

        return chapter;
    }
    
    private int FindTrackNumber(AudiobookFile audiobookFile)
    {
        if (audiobookFile.Metadata?.Track is not null)
        {
            return audiobookFile.Metadata.Track ?? 0;
        }
        
        var fileName = audiobookFile.FileName;
        
        var match = Regex.Match(fileName, @"(\d{1,2})[-\s](.*)");
        
        return match.Success ? int.Parse(match.Groups[1].Value) : 0;
    }
}