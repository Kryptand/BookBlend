namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public class FileNameHelper
{
    public string? GetParentDirectoryName(string filePath)
    {
        var pathParts = filePath.Split(Path.DirectorySeparatorChar);
        return pathParts.Length > 1 ? pathParts[^2] : null;
    }

    public string? GetGrandParentDirectoryName(string filePath)
    {
        var pathParts = filePath.Split(Path.DirectorySeparatorChar);
        return pathParts.Length > 2 ? pathParts[^3] : null;
    }

    public bool IsCdDirectoryName(string? directoryName)
    {
        return directoryName != null && directoryName.Contains("cd", StringComparison.CurrentCultureIgnoreCase);
    }

    public string GetFileNameWithoutExtension(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return string.Empty;
        return !fileName.Contains('.') ? fileName : fileName[..fileName.LastIndexOf('.')];
    }
}