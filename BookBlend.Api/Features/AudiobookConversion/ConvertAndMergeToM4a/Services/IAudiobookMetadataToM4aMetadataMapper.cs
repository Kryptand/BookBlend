using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services;

public interface IAudiobookMetadataToM4AMetadataMapper
{
    string GenerateOutputFileName(Audiobook audiobook);
    string GenerateM4AMetadata(Audiobook audiobook);
    void CopyMetadata(Audiobook sourceMetadata, string targetFile);
}