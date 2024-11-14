using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services;

public interface ICombineFilesIntoM4AService
{
    Task<string> MergeMp3ToM4A(IEnumerable<string> mp3Files, string outputFilePath, Audiobook audiobook);
}