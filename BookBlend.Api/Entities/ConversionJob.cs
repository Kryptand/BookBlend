namespace BookBlend.Api.Entities;

public enum ConversionJobStatus
{
    Pending,
    InProgress,
    Cancelled,
    Completed,
    Failed
}

public class ConversionJob
{
    public Guid Id { get; set; }
    
    public List<Guid> AudiobookIdsToConvert { get; set; } = new();
    
    public string ConfiguredOutputDirectory { get; set; }
    
    public ConversionJobStatus Status { get; set; } = ConversionJobStatus.Pending;
    
    public Guid? CurrentAudiobookId { get; set; }
    
    public string? ErrorMessage { get; set; } 
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? StartedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? FinishedAt { get; set; }
}
