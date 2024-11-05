using System.ComponentModel.DataAnnotations;

namespace BookBlend.Api.Entities
{
    public class Chapter
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public Guid AudiobookId { get; set; }

        public Audiobook Audiobook { get; set; }

        public Guid AudioFileId { get; set; }

        public AudiobookFile AudioFile { get; set; }

        public int TrackNumber { get; set; }
    }
}