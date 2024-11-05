using System.ComponentModel.DataAnnotations;

namespace BookBlend.Api.Entities;

public sealed class AudiobookFile
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string FileName { get; set; }
    
    [Required]
    [MaxLength(1024)]
    public string FilePath { get; set; }
    
    [MaxLength(10)]
    public string FileExtension { get; set; }
    
    [MaxLength(255)]
    public string FileSize { get; set; }
    
    [MaxLength(255)]
    public string Duration { get; set; }
    public FileMetadata? Metadata { get; set; }
}