using BookBlend.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Database;

public class AudiobookDbContext(DbContextOptions<AudiobookDbContext> options) : DbContext(options)
{
    public DbSet<Audiobook> Audiobooks { get; set; }
    public DbSet<AudiobookFile> AudiobookFiles { get; set; }
    public DbSet<Chapter> Chapters { get; set; }

    public DbSet<FileMetadata> FileMetadata { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateAudiobookChapterConfiguration(modelBuilder);
        CreateChapterAudioFileConfiguration(modelBuilder);
        CreateAudiobookFileConfiguration(modelBuilder);
        CreateFileMetadataConfiguration(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }


    private static void CreateAudiobookChapterConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audiobook>().HasKey(a => a.Id);
        modelBuilder.Entity<Audiobook>().HasMany(a => a.Chapters).WithOne().HasForeignKey(c => c.AudiobookId);
    }

    private static void CreateChapterAudioFileConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chapter>().HasKey(c => c.Id);
        modelBuilder.Entity<Chapter>()
            .HasOne(c => c.Audiobook)
            .WithMany(a => a.Chapters)
            .HasForeignKey(c => c.AudiobookId);
        modelBuilder.Entity<Chapter>().HasIndex(c => c.AudioFileId).IsUnique();
    }

    private static void CreateAudiobookFileConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AudiobookFile>().HasKey(af => af.Id);
        modelBuilder.Entity<AudiobookFile>().HasOne<FileMetadata>(af => af.Metadata).WithOne()
            .HasForeignKey<AudiobookFile>(af => af.Id);
        modelBuilder.Entity<AudiobookFile>().HasIndex(af => af.FilePath).IsUnique();
    }

    private static void CreateFileMetadataConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileMetadata>().HasKey(fm => fm.Id);
    }
}