namespace BookBlend.Api.Shared;

public sealed class FileSystemWrapper : IFileSystemWrapper
{
    public bool DirectoryExists(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }

    public string[] GetFiles(string directoryPath, string searchPattern)
    {
        return Directory.GetDirectories(directoryPath, searchPattern);
    }

    public string[] GetFiles(string directoryPath, string searchPattern, SearchOption searchOption)
    {
        return Directory.GetFiles(directoryPath, searchPattern, searchOption);
    }

    public string[] GetFiles(string directoryPath)
    {
        return Directory.GetDirectories(directoryPath);
    }


    public void CleanUpTemporaryFile(string filePath)
    {
        if (File.Exists(filePath)) File.Delete(filePath);
    }
}