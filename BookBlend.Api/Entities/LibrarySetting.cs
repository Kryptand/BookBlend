using System.ComponentModel.DataAnnotations;

namespace BookBlend.Api.Entities;

public sealed class LibrarySettings
{
    public Guid Id { get; set; }
    
    public ICollection<LibraryPath> Paths { get; set; } = new List<LibraryPath>();

    public string DefaultLanguage { get; set; } = "de";
    
    [Required]
    public string OutputDirectory { get; set; }
}

