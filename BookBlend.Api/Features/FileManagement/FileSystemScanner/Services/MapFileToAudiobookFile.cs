using BookBlend.Api.Entities;
using TagLib;
using File = TagLib.File;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner.Services;

public sealed class MapFileToAudiobookFile : IMapFileToAudiobookFile
{
    private const string DefaultLanguage = "de";

    public AudiobookFile MapToAudiobookFile(string filePath)
    {
        var tagLibFile = File.Create(filePath);
        var tagMetadata = tagLibFile.GetTag(TagTypes.Id3v2);

        return new AudiobookFile
        {
            Id = Guid.NewGuid(),
            FileName = Path.GetFileNameWithoutExtension(filePath),
            FilePath = filePath,
            FileExtension = Path.GetExtension(filePath),
            FileSize = new FileInfo(filePath).Length.ToString(),
            Duration = tagLibFile.Properties.Duration.ToString(),
            Metadata = MapToMetadata(tagMetadata, tagLibFile)
        };
    }

    private static FileMetadata MapToMetadata(Tag tagMetadata, File tagLibFile)
    {
        return new FileMetadata
        {
            Id = Guid.NewGuid(),
            Authors = tagMetadata?.Performers?.ToList() ?? new List<string>(),
            Narrators = tagMetadata?.AlbumArtists?.ToList() ?? new List<string>(),
            Publisher = tagMetadata?.Publisher ?? string.Empty,
            ReleaseDate = tagMetadata?.Year.ToString() ?? string.Empty,
            Comment = tagMetadata?.Comment ?? string.Empty,
            Images = tagLibFile?.Tag?.Pictures?.Length > 0
                ? ConvertImagesToBase64WithMimeType(tagLibFile.Tag.Pictures)
                : new List<string>(),
            Language = DefaultLanguage,
            Description = tagMetadata?.Description ?? string.Empty,
            Genres = tagMetadata?.Genres?.ToList() ?? new List<string>(),
            Track = GetTrackNumber(tagMetadata?.Track),
            Album = tagMetadata?.Album ?? string.Empty
        };
    }

    private static int GetTrackNumber(uint? trackNumber)
    {
        return trackNumber.HasValue && trackNumber.Value != 0 ? (int)trackNumber.Value : 0;
    }

    private static List<string> ConvertImagesToBase64WithMimeType(IPicture[] pictures)
    {
        return pictures.Select(picture =>
        {
            var base64Image = Convert.ToBase64String(picture.Data.Data);
            return $"data:{picture.MimeType};base64,{base64Image}";
        }).ToList();
    }
}