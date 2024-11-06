namespace BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Services;

public interface ILibraryPathValidatorService
{
    bool DirectoryExists(string path);
    bool ValidatePath(string path);
}