using BookBlend.Api.Database;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner.Commands;

public sealed class ScanLibraryForAudiobooksCommandResult
{
    public int AddedFiles { get; set; } = 0;
    public int DeletedFiles { get; set; } = 0;
}

public sealed class ScanLibraryForAudiobooksCommand : IRequest<Result<ScanLibraryForAudiobooksCommandResult>>;

public sealed class ScanLibraryForAudiobooksCommandValidator : AbstractValidator<ScanLibraryForAudiobooksCommand>
{
    private readonly AudiobookDbContext _audiobookDbContext;

    public ScanLibraryForAudiobooksCommandValidator(AudiobookDbContext dbContext)
    {
        _audiobookDbContext = dbContext;
        
        RuleFor(c => c).MustAsync(ValidateLibraryExists).WithMessage("Library does not exist");
    }

    private Task<bool> ValidateLibraryExists(ScanLibraryForAudiobooksCommand arg1, CancellationToken arg2)
    {
        return _audiobookDbContext.LibrarySettings.AnyAsync(arg2);
    }
}