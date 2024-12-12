using BookBlend.Api.Database;
using BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Features.AudiobookConversion.ConvertAndMergeToM4a;

public class PeriodicallyCheckForNewConversionJobs(
    IServiceScopeFactory scopeFactory,
    ILogger<PeriodicallyCheckForNewConversionJobs> logger)
    : BackgroundService
{
    private const int IntervalInSeconds = 60;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessPendingConversionJobs(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(IntervalInSeconds), stoppingToken);
        }
    }

    private async Task ProcessPendingConversionJobs(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<AudiobookDbContext>();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        var conversionJobs = await context.ConversionJobs
            .Where(x => x.Status == Entities.ConversionJobStatus.Pending)
            .ToListAsync(stoppingToken);

        foreach (var conversionJob in conversionJobs)
        {
            var command = new ConvertAndMergeAudiobooksToM4ACommand { ConversionJobId = conversionJob.Id };
            await sender.Send(command, stoppingToken);
                
            logger.LogInformation($"Conversion job {conversionJob.Id} has been started");
        }
    }
}