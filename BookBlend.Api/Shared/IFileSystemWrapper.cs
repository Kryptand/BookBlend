namespace BookBlend.Api.Shared;

public interface IFileSystemWrapper
{
    bool DirectoryExists(string directoryPath);
    string[] GetFiles(string directoryPath, string searchPattern);
    string[] GetFiles(string directoryPath, string searchPattern, SearchOption searchOption);
    string[] GetFiles(string directoryPath);
    void CleanUpTemporaryFile(string filePath);
}