namespace BookBlend.Api.Entities;

public sealed class LibrarySettings
{
    public Guid Id { get; set; }
    
    public List<LibraryPath> Paths { get; set; } = new();

    public string DefaultLanguage { get; set; } = "de";
}