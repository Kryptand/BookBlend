using System.Text;
using BookBlend.Api.Entities;
using File = TagLib.File;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services;

public class AudiobookMetadataToM4AMetadataMapper : IAudiobookMetadataToM4AMetadataMapper
{
    public string GenerateOutputFileName(Audiobook audiobook)
    {
        return audiobook.Title?.ToLower().Replace(" ", "-").Replace(":", "").Replace("?", "") ??
               $"audiobook-{audiobook.Id}";
    }

    public string GenerateM4AMetadata(Audiobook audiobook)
    {
        var metadataBuilder = new StringBuilder();
        metadataBuilder.AppendLine(";FFMETADATA1");
        var totalMilliseconds = 0;

        foreach (var chapter in audiobook.Chapters)
        {
            var durationMilliseconds = ConvertDurationToMilliseconds(chapter.Duration);
            var endMilliseconds = totalMilliseconds + durationMilliseconds;
            metadataBuilder.AppendLine("[CHAPTER]");
            metadataBuilder.AppendLine("TIMEBASE=1/1000");
            metadataBuilder.AppendLine($"START={totalMilliseconds}");
            metadataBuilder.AppendLine($"END={endMilliseconds}");
            metadataBuilder.AppendLine($"title={chapter.Title}");
            totalMilliseconds = endMilliseconds;
        }

        return metadataBuilder.ToString();
    }

    public void CopyMetadata(Audiobook sourceMetadata, string targetFile)
    {
        var year= sourceMetadata.ReleaseDate?.Year ?? 0;
        
        var targetMeta = File.Create(targetFile);
        targetMeta.Tag.Title = sourceMetadata.Title;
        targetMeta.Tag.Album = sourceMetadata.Title;
        targetMeta.Tag.Performers = sourceMetadata.Authors.ToArray();
        targetMeta.Tag.AlbumArtists = sourceMetadata.Narrators.ToArray();
        targetMeta.Tag.Composers = sourceMetadata.Authors.ToArray();
        targetMeta.Tag.Genres = sourceMetadata.Genre.ToArray();
        targetMeta.Tag.Year = (uint)year;
        targetMeta.Tag.Comment = sourceMetadata.Description;

        // TODO: Add support for cover images
        // targetMeta.Tag.Pictures = sourceMetadata.CoverImage != null
        //    ? new IPicture[] { new Picture(sourceMetadata.CoverImage) }
        //    : Array.Empty<Picture>();

        targetMeta.Save();
    }
    

    private int ConvertDurationToMilliseconds(TimeSpan duration)
    {
        return (int)duration.TotalMilliseconds;
    }
}