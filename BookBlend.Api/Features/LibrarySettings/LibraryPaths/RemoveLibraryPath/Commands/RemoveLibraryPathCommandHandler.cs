using BookBlend.Api.Database;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.RemoveLibraryPath.Commands;

public sealed class RemoveLibraryPathCommandHandler(
    AudiobookDbContext dbContext,
    IValidator<RemoveLibraryPathCommand> validator)
    : IRequestHandler<RemoveLibraryPathCommand, Result>
{

    public async Task<Result> Handle(RemoveLibraryPathCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return await Task.FromResult(Result.Failure(new Error(
                "RemoveLibraryPath.Validation",
                validationResult.ToString())));
        }

        await RemoveLibraryPathFromDatabaseAsync(request.Id);

        return await Task.FromResult(Result.Success());

    }

    private async Task RemoveLibraryPathFromDatabaseAsync(Guid requestId)
    {
        var libraryPath =await dbContext.LibraryPaths.FindAsync(requestId);

        dbContext.LibraryPaths.Remove(libraryPath);

        dbContext.SaveChangesAsync();
    }
}