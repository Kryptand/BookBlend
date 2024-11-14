using BookBlend.Api.Shared;
using MediatR;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner.Commands;

public sealed class ScanLibraryForAudiobooksCommandResult
{
    public int AddedFiles { get; set; } = 0;
    public int DeletedFiles { get; set; } = 0;
}

public sealed class ScanLibraryForAudiobooksCommand : IRequest<Result<ScanLibraryForAudiobooksCommandResult>>;
