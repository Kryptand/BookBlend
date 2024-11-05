using BookBlend.Api.Entities;
using BookBlend.Api.Shared;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner.Services;

public sealed class FileScannerService(
    IFileSystemWrapper fileSystemWrapper,
    IMapFileToAudiobookFile mapFileToAudiobookFile)
    : IFileScannerService
{
    private static readonly string[] AudiobookFileExtensions = { ".mp3", ".aac" };

    public async Task<IEnumerable<AudiobookFile>> ScanDirectoryForAudiobooks(string directoryPath)
    {
        var audiobookFiles = new List<AudiobookFile>();
        
        var files = fileSystemWrapper.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var fileExtension = Path.GetExtension(file);
            
            if (!AudiobookFileExtensions.Contains(fileExtension))
            {
                continue;
            }
            
            var audiobookFile = mapFileToAudiobookFile.MapToAudiobookFile(file);
            audiobookFiles.Add(audiobookFile);
        }

        return await Task.FromResult(audiobookFiles.AsEnumerable());
    }
}