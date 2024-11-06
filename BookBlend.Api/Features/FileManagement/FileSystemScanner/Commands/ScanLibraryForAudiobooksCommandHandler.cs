using BookBlend.Api.Database;
using BookBlend.Api.Entities;
using BookBlend.Api.Features.FileManagement.FileSystemScanner.Services;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner.Commands
{
    public class ScanLibraryForAudiobooksCommandHandler(
        IFileScannerService fileScannerService,
        IValidator<ScanLibraryForAudiobooksCommand> validator,
        AudiobookDbContext dbContext) : IRequestHandler<ScanLibraryForAudiobooksCommand, Result<ScanLibraryForAudiobooksCommandResult>>
    {
        public async Task<Result<ScanLibraryForAudiobooksCommandResult>> Handle(ScanLibraryForAudiobooksCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<ScanLibraryForAudiobooksCommandResult>(new Error(
                    "ScanLibraryForAudiobooks.Validation", validationResult.ToString()));
            }

            var libraryPaths = await dbContext.LibraryPaths.ToListAsync(cancellationToken);
            if (!libraryPaths.Any())
            {
                return Result.Failure<ScanLibraryForAudiobooksCommandResult>(new Error(
                    "ScanLibraryForAudiobooks.Validation", "No library paths are set"));
            }

            var paths = libraryPaths.Select(lp => lp.Path).ToList();
            var totalNewFiles = 0;
            var totalDeletedFiles = 0;

            foreach (var libraryPath in paths)
            {
                var scannedFiles = await fileScannerService.ScanDirectoryForAudiobooks(libraryPath);
                var existingFilePaths = await GetExistingFilePaths(cancellationToken);
                var existingFiles = await dbContext.AudiobookFiles.ToListAsync(cancellationToken);

                var newFiles = IdentifyNewFiles(scannedFiles, existingFilePaths);
                var deletedFiles = IdentifyDeletedFiles(existingFiles, scannedFiles);

                if (newFiles.Any())
                {
                    dbContext.AudiobookFiles.AddRange(newFiles);
                    totalNewFiles += newFiles.Count;
                }
                if (deletedFiles.Any())
                {
                    dbContext.AudiobookFiles.RemoveRange(deletedFiles);
                    totalDeletedFiles += deletedFiles.Count;
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(new ScanLibraryForAudiobooksCommandResult
            {
                AddedFiles = totalNewFiles,
                DeletedFiles = totalDeletedFiles
            });
        }

        private static List<AudiobookFile> IdentifyNewFiles(IEnumerable<AudiobookFile> files, HashSet<string> existingFilePaths)
        {
            return files.Where(f => !existingFilePaths.Contains(f.FilePath)).ToList();
        }

        private static List<AudiobookFile> IdentifyDeletedFiles(IEnumerable<AudiobookFile> existingFiles, IEnumerable<AudiobookFile> scannedFiles)
        {
            var scannedFilePaths = new HashSet<string>(scannedFiles.Select(f => f.FilePath));
            return existingFiles.Where(f => !scannedFilePaths.Contains(f.FilePath)).ToList();
        }

        private async Task<HashSet<string>> GetExistingFilePaths(CancellationToken cancellationToken)
        {
            return
            [

                ..await dbContext.AudiobookFiles
                    .Select(af => af.FilePath)
                    .ToListAsync(cancellationToken)
            ];
        }
    }
}