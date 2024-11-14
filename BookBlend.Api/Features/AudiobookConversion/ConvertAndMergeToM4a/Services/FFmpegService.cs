using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services
{
    public class FFmpegService : IFFmpegService
    {
        private static readonly string FFmpegPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ffmpeg");

        public FFmpegService()
        {
            EnsureFFmpegDownloaded();
        }

        public async Task ConcatenateMp3Files(string concatFilePath, string tempMp3File)
        {
            await FFmpeg.Conversions.New()
                .AddParameter($"-f concat -safe 0 -i \"{concatFilePath}\" -c copy \"{tempMp3File}\"")
                .Start();
        }

        public async Task<string> ConcatenateM4AFiles(IEnumerable<string> m4AFilePaths, string outputM4AFilePath)
        {
            using var tempFile = new TempFile();
            await CreateConcatListFileAsync(m4AFilePaths, tempFile.FilePath);

            await FFmpeg.Conversions.New()
                .AddParameter("-f concat", ParameterPosition.PreInput)
                .AddParameter($"-i \"{tempFile.FilePath}\"")
                .AddParameter("-c copy")
                .AddParameter("-y")
                .SetOutput(outputM4AFilePath)
                .Start();

            return outputM4AFilePath;
        }

        public bool IsFFmpegDownloaded()
        {
            var ffmpegExecutable = Path.Combine(FFmpegPath, "ffmpeg");
            return File.Exists(ffmpegExecutable);
        }

        public async Task AddMetadataToM4A(string inputM4AFilePath, string chapterMetadata)
        {
            using var metadataTempFile = new TempFile();
            await File.WriteAllTextAsync(metadataTempFile.FilePath, chapterMetadata);

            await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputM4AFilePath}\"")
                .AddParameter($"-i \"{metadataTempFile.FilePath}\"")
                .AddParameter("-c:v copy")
                .AddParameter("-map_metadata 1")
                .SetOutput(inputM4AFilePath)
                .SetOverwriteOutput(true)
                .Start();
        }

        public async Task<string> ConvertMp3ToM4A(string inputMp3FilePath, string outputM4AFilePath)
        {
            await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputMp3FilePath}\"")
                .AddParameter("-c:a aac -b:a 256k -ar 44100 -ac 2")
                .AddParameter("-c:v copy")
                .SetOutput(outputM4AFilePath)
                .SetOverwriteOutput(true)
                .Start();

            return outputM4AFilePath;
        }

        private void EnsureFFmpegDownloaded()
        {
            if (!IsFFmpegDownloaded())
            {
                FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, FFmpegPath).Wait();
            }

            FFmpeg.SetExecutablesPath(FFmpegPath);
        }

        private async Task CreateConcatListFileAsync(IEnumerable<string> filePaths, string listFilePath)
        {
            await using var writer = new StreamWriter(listFilePath);
            foreach (var filePath in filePaths)
            {
                await writer.WriteLineAsync($"file '{Path.GetFullPath(filePath)}'");
            }
        }
    }
}