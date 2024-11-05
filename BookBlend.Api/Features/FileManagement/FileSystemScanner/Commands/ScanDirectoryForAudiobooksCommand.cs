using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner.Commands;

public sealed class ScanDirectoryForAudiobooksCommandResult
{
    public int AddedFiles { get; set; } = 0;
    public int DeletedFiles { get; set; } = 0;
}

public sealed class ScanDirectoryForAudiobooksCommand(string directoryPath) : IRequest<Result<ScanDirectoryForAudiobooksCommandResult>>
{
    public string DirectoryPath { get; } = directoryPath;
}

public sealed class ScanDirectoryForAudiobooksCommandValidator : AbstractValidator<ScanDirectoryForAudiobooksCommand>
{
private readonly IFileSystemWrapper _fileSystemWrapper;

    public ScanDirectoryForAudiobooksCommandValidator(IFileSystemWrapper fileSystemWrapper)
    {
        _fileSystemWrapper = fileSystemWrapper;

        RuleFor(c => c.DirectoryPath).NotEmpty().WithMessage("Directory path cannot be null or empty");
        RuleFor(c => c.DirectoryPath).Must(ValidateDirectoryPath).WithMessage("Directory does not exist");
    }

    private bool ValidateDirectoryPath(string arg)
    {
        return _fileSystemWrapper.DirectoryExists(arg);
    }
}