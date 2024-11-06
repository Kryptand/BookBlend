using BookBlend.Api.Database;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.UpdateLibraryPath.Commands;

public sealed class UpdateLibraryPathCommandHandler(
    AudiobookDbContext dbContext,
    IValidator<UpdateLibraryPathCommand> validator)
    : IRequestHandler<UpdateLibraryPathCommand, Result>
{

    public async Task<Result> Handle(UpdateLibraryPathCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return await Task.FromResult(Result.Failure(new Error(
                "UpdateLibraryPath.Validation",
                validationResult.ToString())));
        }

        await UpdateLibraryPathInDatabaseAsync(request.Id, request.Path);

        return await Task.FromResult(Result.Success());
    }

    private async Task UpdateLibraryPathInDatabaseAsync(Guid requestId, string path)
    {
        var libraryPath = await dbContext.LibraryPaths.FindAsync(requestId);
        
        libraryPath.Path = path;

        dbContext.LibraryPaths.Update(libraryPath);

        await dbContext.SaveChangesAsync();
    }
}