using BookBlend.Api.Database;
using BookBlend.Api.Entities;
using BookBlend.Api.Features.FileManagement.FileSystemScanner.Services;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner.Commands;

public class ScanDirectoryForAudiobooksCommandHandler(
    IFileScannerService fileScannerService,
    IValidator<ScanDirectoryForAudiobooksCommand> validator,
    AudiobookDbContext dbContext) : IRequestHandler<ScanDirectoryForAudiobooksCommand, Result<ScanDirectoryForAudiobooksCommandResult>>
{
    public async Task<Result<ScanDirectoryForAudiobooksCommandResult>> Handle(ScanDirectoryForAudiobooksCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Failure<ScanDirectoryForAudiobooksCommandResult>(new Error(
                "ScanDirectoryForAudiobooks.Validation", validationResult.ToString()));
        }

        var scannedFiles = await fileScannerService.ScanDirectoryForAudiobooks(request.DirectoryPath);
        var existingFilePaths = await GetExistingFilePaths(cancellationToken);

        var existingFiles = await dbContext.AudiobookFiles.ToListAsync(cancellationToken);

        var newFiles = IdentifyNewFiles(scannedFiles, existingFilePaths);

        var deletedFiles = IdentifyDeletedFiles(existingFiles, scannedFiles);

        if (!newFiles.Any() && !deletedFiles.Any())
        {
            return Result.Success(new ScanDirectoryForAudiobooksCommandResult());
        }

        if (newFiles.Any())
        {
            dbContext.AudiobookFiles.AddRange(newFiles);
        }

        if (deletedFiles.Any())
        {
            dbContext.AudiobookFiles.RemoveRange(deletedFiles);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success(new ScanDirectoryForAudiobooksCommandResult
        {
            AddedFiles = newFiles.Count,
            DeletedFiles = deletedFiles.Count
        });
    }

    private static List<AudiobookFile> IdentifyNewFiles(
        IEnumerable<AudiobookFile> files, HashSet<string> existingFilePaths)
    {
        var newFiles = files.Where(f => !existingFilePaths.Contains(f.FilePath)).ToList();
        
        return newFiles;
    }

    private static List<AudiobookFile> IdentifyDeletedFiles(
        IEnumerable<AudiobookFile> existingFiles, IEnumerable<AudiobookFile> scannedFiles)
    {
        var scannedFilePaths = new HashSet<string>(scannedFiles.Select(f => f.FilePath));
        var deletedFiles = existingFiles.Where(f => !scannedFilePaths.Contains(f.FilePath)).ToList();
        
        return deletedFiles;
    }

    private async Task<HashSet<string>> GetExistingFilePaths(CancellationToken cancellationToken)
    {
        var existingFilePaths = new HashSet<string>(await dbContext.AudiobookFiles
            .Select(af => af.FilePath)
            .ToListAsync(cancellationToken));
        
        return existingFilePaths;
    }
}