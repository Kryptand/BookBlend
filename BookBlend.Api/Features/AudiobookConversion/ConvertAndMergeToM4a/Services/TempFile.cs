namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services;

public class TempFile : IDisposable
{
    public string FilePath { get; } = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".tmp");

    public void Dispose()
    {
        if (File.Exists(FilePath)) File.Delete(FilePath);
    }
}