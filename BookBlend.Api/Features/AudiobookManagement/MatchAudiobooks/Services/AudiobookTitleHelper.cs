using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public class AudiobookTitleHelper(FileNameHelper fileNameHelper)
{
    public string DetermineAudiobookTitle(AudiobookFile audiobookFile, string chapterName)
    {
        var audiobookTitleFromTag = audiobookFile.Metadata?.Album;

        if (audiobookTitleFromTag != null)
        {
            return audiobookTitleFromTag;
        }
        
        var filePath = audiobookFile.FilePath;
        
        var parentDirectoryName = fileNameHelper.GetParentDirectoryName(filePath);

        if (string.IsNullOrEmpty(parentDirectoryName))
        {
            return chapterName;
        }
        
        var grandParentDirectoryName = fileNameHelper.GetGrandParentDirectoryName(filePath);
        
        return (fileNameHelper.IsCdDirectoryName(parentDirectoryName)
            ? grandParentDirectoryName
            : parentDirectoryName) ?? throw new InvalidOperationException();
    }
}