namespace BookBlend.Api.Entities;

public sealed class LibraryPath
{
    public Guid Id { get; set; }
    
    public string Path { get; set; }
    
    public Guid LibrarySettingsId { get; set; }
    
    public LibrarySettings? LibrarySettings { get; set; }
}