using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner.Services;

public interface IFileScannerService
{
    Task<IEnumerable<AudiobookFile>> ScanDirectoryForAudiobooks(string directoryPath);
}