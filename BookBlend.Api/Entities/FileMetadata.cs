using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookBlend.Api.Entities
{
    public class FileMetadata
    {
        public Guid Id { get; set; }

        public List<string> Genres { get; set; } = [];

        public List<string> Authors { get; set; } = [];

        public List<string> Narrators { get; set; } = [];

        public int? Track { get; set; }

        [MaxLength(200)]
        public string? Publisher { get; set; }

        [MaxLength(20)]
        public string? ReleaseDate { get; set; }

        [Required]
        [MaxLength(280)]
        public string Language { get; set; }

        public string? Description { get; set; }

        public List<string> Images { get; set; } = [];

        [MaxLength(1024)]
        public string? Comment { get; set; }

        [MaxLength(200)]
        public string? Album { get; set; }
    }
}