using System.Diagnostics;
using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services;

public sealed class CombineFilesIntoM4AService(
    IFFmpegService ffmpegService,
    IAudiobookMetadataToM4AMetadataMapper metadataService,
    ILogger<CombineFilesIntoM4AService> logger) : ICombineFilesIntoM4AService
{
    public async Task<string> MergeMp3ToM4A(IEnumerable<string> mp3Files, Audiobook audiobook)
    {
        var m4APartPaths = new List<string>();
        
        var tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Output");
        
        if (!Directory.Exists(tempDirectory))
        {
            Directory.CreateDirectory(tempDirectory);
        }
        
        var outputFilePath = tempDirectory;
        try
        {
            var outputFullPath = GetOutputFullPath(outputFilePath, audiobook);

            await ConvertMp3FilesToM4A(mp3Files, m4APartPaths, outputFilePath);

            await ffmpegService.ConcatenateM4AFiles(m4APartPaths, outputFullPath);

            var chapterMetadata = metadataService.GenerateM4AMetadata(audiobook);
            
            await ffmpegService.AddChapterMarksToM4A(outputFullPath, chapterMetadata);
            
            metadataService.AddMetadataToAudiofile(audiobook, outputFullPath);

            return outputFullPath;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during the MP3 to M4A merge process.");
            throw;
        }
        finally
        {
            foreach (var m4APartPath in m4APartPaths)
            {
                File.Delete(m4APartPath);
            }
        }
    }

    private string GetOutputFullPath(string outputFilePath, Audiobook audiobook)
    {
        var outputFileName = metadataService.GenerateOutputFileName(audiobook);
        return Path.Combine(outputFilePath, $"{outputFileName}.m4a");
    }

    private async Task ConvertMp3FilesToM4A(IEnumerable<string> mp3Files, List<string> m4APartPaths,
        string outputFilePath)
    {
        var tasks = new List<Task>();

        Parallel.ForEach(mp3Files, mp3File =>
        {
            tasks.Add(Task.Run(async () =>
            {
                var tempM4AFilePath = $"{outputFilePath}/{Guid.NewGuid()}.m4a";
                var m4APartPath = await ffmpegService.ConvertMp3ToM4A(mp3File, tempM4AFilePath);
                lock (m4APartPaths)
                {
                    m4APartPaths.Add(m4APartPath);
                }
            }));
        });

        await Task.WhenAll(tasks);
    }
}