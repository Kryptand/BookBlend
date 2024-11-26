namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services;

public class TempFile:IDisposable
{
    private const string DefaultDirectory = "/Users/andreas.goetze/RiderProjects/BookBlend/BookBlend.Api/Output";
    public string FilePath { get; } = Path.Combine(DefaultDirectory, $"{Guid.NewGuid()}.txt");

    public void Dispose()
    {
        if (File.Exists(FilePath)) File.Delete(FilePath);
    }
}