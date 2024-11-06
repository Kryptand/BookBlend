using BookBlend.Api.Database;
using BookBlend.Api.Entities;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.AddLibraryPath.Commands;

public sealed class AddLibraryPathCommandHandler(
    AudiobookDbContext dbContext,
    IValidator<AddLibraryPathCommand> validator)
    : IRequestHandler<AddLibraryPathCommand, Result>
{

    public async Task<Result> Handle(AddLibraryPathCommand request, CancellationToken cancellationToken)
    {
      var validationResult = validator.Validate(request);
      
        if (!validationResult.IsValid)
        {
            return await Task.FromResult(Result.Failure(new Error(
                "AddLibraryPath.Validation",
                validationResult.ToString())));
        }
        
        await AddLibraryPathToDatabaseAsync(request.Path);

        return await Task.FromResult(Result.Success());
    }

    private async Task AddLibraryPathToDatabaseAsync(string path)
    {
        var libraryPath = new LibraryPath
        {
            Path = path
        };
        
        await dbContext.LibraryPaths.AddAsync(libraryPath);
        
        await dbContext.SaveChangesAsync();
    }
}