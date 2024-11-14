namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services;

public interface IFFmpegService
{
    Task ConcatenateMp3Files(string concatFilePath, string tempMp3File);
    Task<string> ConcatenateM4AFiles(IEnumerable<string> m4AFilePaths, string outputM4AFilePath);
    Task<string> ConvertMp3ToM4A(string inputMp3FilePath, string outputM4AFilePath);
    bool IsFFmpegDownloaded();
    Task AddMetadataToM4A(string inputM4AFilePath, string chapterMetadata);
}