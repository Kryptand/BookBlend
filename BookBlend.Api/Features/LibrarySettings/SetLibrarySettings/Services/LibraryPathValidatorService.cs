using BookBlend.Api.Database;
using BookBlend.Api.Shared;

namespace BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Services;

public sealed class LibraryPathValidatorService(AudiobookDbContext dbContext, IFileSystemWrapper fileSystemWrapper)
    : ILibraryPathValidatorService
{
    public bool DirectoryExists(string path)
    {
        return fileSystemWrapper.DirectoryExists(path);
    }
    
    public bool ValidatePath(string path)
    {
        var existingPaths = dbContext.LibraryPaths
            .Select(p => p.Path)
            .ToList();

        var targetPath = Path.GetFullPath(path);

        return !existingPaths
            .Select(Path.GetFullPath)
            .Any(existingFullPath => PathsConflict(targetPath, existingFullPath));
    }

    private static bool PathsConflict(string path1, string path2)
    {
        return IsSubPath(path1, path2) || IsSubPath(path2, path1) || 
               path1.Equals(path2, StringComparison.OrdinalIgnoreCase);
    }
    
    private static bool IsSubPath(string basePath, string candidatePath)
    {
        var baseUri = new Uri(basePath + Path.DirectorySeparatorChar);
        var candidateUri = new Uri(candidatePath + Path.DirectorySeparatorChar);
        
        return baseUri.IsBaseOf(candidateUri);
    }
}