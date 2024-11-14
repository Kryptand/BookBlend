using BookBlend.Api.Database;
using BookBlend.Api.Entities;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;

namespace BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus.Commands;

public sealed class RegisterAudioobokConversionCommandHandler(
    IValidator<RegisterAudiobookConversionCommand> validator,
    AudiobookDbContext dbContext)
    : IRequestHandler<RegisterAudiobookConversionCommand, Result<Guid>>
{
    public Task<Result<Guid>> Handle(RegisterAudiobookConversionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Task.FromResult(Result.Failure<Guid>(new Error("AudiobookConversionStatus.Validation",
                validationResult.ToString())));
        }

        var currentOutPutDirectory = dbContext.LibrarySettings.FirstOrDefault()?.OutputDirectory;
        
        if (string.IsNullOrEmpty(currentOutPutDirectory))
        {
            return Task.FromResult(Result.Failure<Guid>(new Error("AudiobookConversionStatus.Validation",
                "Library settings not found")));
        }
        
        var conversion = new ConversionJob
        {
            Status = ConversionJobStatus.Pending,
            AudiobookIdsToConvert = request.AudiobookIds.ToList(),
            ConfiguredOutputDirectory = currentOutPutDirectory,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        dbContext.ConversionJobs.Add(conversion);

        dbContext.SaveChanges();

        return Task.FromResult(Result.Success(conversion.Id));
    }
}