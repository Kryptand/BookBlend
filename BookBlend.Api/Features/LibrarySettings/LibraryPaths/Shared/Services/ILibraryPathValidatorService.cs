namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.Shared.Services;

public interface ILibraryPathValidatorService
{
    bool DirectoryExists(string path);
    bool ValidatePath(string path);
}