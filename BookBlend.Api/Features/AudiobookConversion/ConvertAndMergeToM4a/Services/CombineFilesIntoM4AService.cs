using System.Diagnostics;
using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services;

public sealed class CombineFilesIntoM4AService(
    IFFmpegService ffmpegService,
    IAudiobookMetadataToM4AMetadataMapper metadataService,
    ILogger<CombineFilesIntoM4AService> logger) : ICombineFilesIntoM4AService
{
    public async Task<string> MergeMp3ToM4A(IEnumerable<string> mp3Files, string outputFilePath, Audiobook audiobook)
    {
        var stopwatch = Stopwatch.StartNew();
        var m4APartPaths = new List<string>();
        try
        {
            var outputFullPath = GetOutputFullPath(outputFilePath, audiobook);

            logger.LogInformation("Converting MP3 files to M4A.");
            await ConvertMp3FilesToM4A(mp3Files, m4APartPaths);

            logger.LogInformation("Concatenating M4A files.");
            await ffmpegService.ConcatenateM4AFiles(m4APartPaths, outputFullPath);

            logger.LogInformation("Adding metadata to M4A file.");
            var chapterMetadata = metadataService.GenerateM4AMetadata(audiobook);
            await ffmpegService.AddMetadataToM4A(outputFullPath, chapterMetadata);

            metadataService.CopyMetadata(audiobook, outputFullPath);

            stopwatch.Stop();
            logger.LogInformation($"MergeMp3ToM4A executed in {stopwatch.Elapsed}.");

            return outputFullPath;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during the MP3 to M4A merge process.");
            throw;
        }
    }

    private string GetOutputFullPath(string outputFilePath, Audiobook audiobook)
    {
        var outputFileName = metadataService.GenerateOutputFileName(audiobook);
        return Path.Combine(outputFilePath, $"{outputFileName}.m4a");
    }

    private async Task ConvertMp3FilesToM4A(IEnumerable<string> mp3Files, List<string> m4APartPaths)
    {
        var convertTasks = mp3Files.Select(async mp3File =>
        {
            var tempM4AFilePath = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(mp3File)}.m4a");
            var m4APartPath = await ffmpegService.ConvertMp3ToM4A(mp3File, tempM4AFilePath);
            m4APartPaths.Add(m4APartPath);
        });

        await Task.WhenAll(convertTasks);
    }
}