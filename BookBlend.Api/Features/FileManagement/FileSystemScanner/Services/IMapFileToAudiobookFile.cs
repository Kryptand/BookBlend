using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner.Services;

public interface IMapFileToAudiobookFile
{
    AudiobookFile MapToAudiobookFile(string filePath);
}