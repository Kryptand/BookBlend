using BookBlend.Api.Database;
using BookBlend.Api.Entities;
using BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Services;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Commands;

public sealed class ConvertAndMergeAudiobooksToM4aCommandHandler(
    ICombineFilesIntoM4AService combineFilesIntoM4AService,
    IValidator<ConvertAndMergeAudiobooksToM4aCommand> validator,
    AudiobookDbContext dbContext) : IRequestHandler<ConvertAndMergeAudiobooksToM4aCommand, Result>
{
    public async Task<Result> Handle(ConvertAndMergeAudiobooksToM4aCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result.Failure<CombinedAudiobooksResult>(
                new Error("ConvertAndMergeAudiobooksToM4a.Validation", validationResult.ToString()));
        }
        
        var conversionJobSettings = await dbContext.ConversionJobs
            .FirstOrDefaultAsync(x => x.Id == request.ConversionJobId, cancellationToken);

        if (conversionJobSettings == null)
        {
            return Result.Failure<CombinedAudiobooksResult>(
                new Error("ConvertAndMergeAudiobooksToM4a.Validation", "Conversion job not found"));
        }

        return await ProcessConversion(conversionJobSettings, cancellationToken);
    }

    private async Task<Result> ProcessConversion(
        ConversionJob conversionJobSettings, CancellationToken cancellationToken)
    {
        var audiobooksToConvert = conversionJobSettings.AudiobookIdsToConvert.ToList();
        
        foreach (var audiobookId in audiobooksToConvert)
        {
            await UpdateConversionJobStatus(conversionJobSettings, audiobookId, ConversionJobStatus.InProgress, cancellationToken);

            var updatedConversionJob = await dbContext.ConversionJobs
                .FirstOrDefaultAsync(x => x.Id == conversionJobSettings.Id, cancellationToken);
            
            if(updatedConversionJob == null)
            {
                return Result.Failure<CombinedAudiobooksResult>(
                    new Error("ConvertAndMergeAudiobooksToM4a.Validation", "Conversion job has been deleted while processing"));
            }
            
            if(updatedConversionJob.Status == ConversionJobStatus.Cancelled)
            {
                await UpdateConversionJobStatus(conversionJobSettings, ConversionJobStatus.Cancelled, cancellationToken);
                
                return Result.Failure<CombinedAudiobooksResult>(
                    new Error("ConvertAndMergeAudiobooksToM4a.Validation", "Conversion job cancelled"));
            }
            
            try
            {
                await MergeMp3FilesIntoM4A(cancellationToken, audiobookId, conversionJobSettings.ConfiguredOutputDirectory);

                conversionJobSettings.AudiobookIdsToConvert.Remove(audiobookId);
                
                await UpdateConversionJobStatus(conversionJobSettings, ConversionJobStatus.InProgress, cancellationToken);
            }
            catch (Exception ex)
            {
                await UpdateConversionJobErrorStatus(conversionJobSettings, ex.Message, cancellationToken);
                
                return Result.Failure<CombinedAudiobooksResult>(
                    new Error("ConvertAndMergeAudiobooksToM4a.Validation", ex.Message));
            }
        }

        await UpdateFinalConversionJobStatus(conversionJobSettings, ConversionJobStatus.Completed, cancellationToken);
        
        return Result.Success();
    }

    private async Task MergeMp3FilesIntoM4A(
        CancellationToken cancellationToken, Guid audiobookId, string outputDirectory)
    {
        var audiobook = await GetAudiobookAsync(audiobookId, cancellationToken);
        if (audiobook == null) return;

        var chapters = await GetChaptersAsync(audiobookId, cancellationToken);
        if (!chapters.Any()) return;

        var files = chapters.Select(x => x.AudioFile?.FilePath)
            .Where(filePath => !string.IsNullOrEmpty(filePath))
            .ToList() as List<string>;

        if (!files.Any()) return;

        var tempLocation = await combineFilesIntoM4AService.MergeMp3ToM4A(files, audiobook);
        
        try
        {
            var outputFilePath = Path.Combine(outputDirectory, $"{audiobook.Title}.m4a");
            File.Move(tempLocation, outputFilePath);
        }
        catch (Exception ex)
        {
            File.Delete(tempLocation);
            throw new Exception("Failed to move the file to the output directory", ex);
        }
    }

    private async Task UpdateConversionJobStatus(
        ConversionJob conversionJobSettings, Guid audiobookId, ConversionJobStatus status, CancellationToken cancellationToken)
    {
        conversionJobSettings.CurrentAudiobookId = audiobookId;
        conversionJobSettings.Status = status;
        conversionJobSettings.UpdatedAt = DateTime.UtcNow;

        dbContext.ConversionJobs.Update(conversionJobSettings);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateConversionJobStatus(
        ConversionJob conversionJobSettings, ConversionJobStatus status, CancellationToken cancellationToken)
    {
        conversionJobSettings.Status = status;
        conversionJobSettings.UpdatedAt = DateTime.UtcNow;

        dbContext.ConversionJobs.Update(conversionJobSettings);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateConversionJobErrorStatus(
        ConversionJob conversionJobSettings, string errorMessage, CancellationToken cancellationToken)
    {
        conversionJobSettings.Status = ConversionJobStatus.Failed;
        conversionJobSettings.ErrorMessage = errorMessage;
        conversionJobSettings.UpdatedAt = DateTime.UtcNow;
        conversionJobSettings.FinishedAt = DateTime.UtcNow;

        dbContext.ConversionJobs.Update(conversionJobSettings);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateFinalConversionJobStatus(
        ConversionJob conversionJobSettings, ConversionJobStatus status, CancellationToken cancellationToken)
    {
        conversionJobSettings.Status = status;
        conversionJobSettings.UpdatedAt = DateTime.UtcNow;
        conversionJobSettings.FinishedAt = DateTime.UtcNow;

        dbContext.ConversionJobs.Update(conversionJobSettings);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<Chapter>> GetChaptersAsync(Guid audiobookId, CancellationToken cancellationToken)
    {
        return await dbContext.Chapters
            .Include(x => x.AudioFile)
            .Where(x => x.AudiobookId == audiobookId)
            .OrderBy(x => x.TrackNumber)
            .ToListAsync(cancellationToken);
    }

    private async Task<Audiobook?> GetAudiobookAsync(Guid audiobookId, CancellationToken cancellationToken)
    {
        return await dbContext.Audiobooks
            .FirstOrDefaultAsync(x => x.Id == audiobookId, cancellationToken);
    }
}