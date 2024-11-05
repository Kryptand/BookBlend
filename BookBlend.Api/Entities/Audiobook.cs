using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBlend.Api.Entities;

 public class Audiobook
{
    public Guid Id { get; set; }

    [MaxLength(200)]
    public string? Title { get; set; }

    public TimeSpan Duration { get; set; }

    [MaxLength(200)]
    public string? Publisher { get; set; }

    public DateTime? ReleaseDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string Language { get; set; }

    public string? Description { get; set; }

    public string? CoverImage { get; set; }

    public int TotalTracks { get; set; }

    public List<string> Genre { get; set; } = new();
    public List<Chapter> Chapters { get; set; } = new();
    public List<string> Authors { get; set; } = new();
    public List<string> Narrators { get; set; } = new();
}
