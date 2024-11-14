using BookBlend.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Database;

public class AudiobookDbContext(DbContextOptions<AudiobookDbContext> options) : DbContext(options)
{
    public DbSet<Audiobook> Audiobooks { get; set; }
    public DbSet<AudiobookFile> AudiobookFiles { get; set; }
    public DbSet<Chapter> Chapters { get; set; }

    public DbSet<FileMetadata> FileMetadata { get; set; }
    public DbSet<LibraryPath> LibraryPaths { get; set; }
    public DbSet<LibrarySettings> LibrarySettings { get; set; }
    public DbSet<ConversionJob> ConversionJobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateAudiobookChapterConfiguration(modelBuilder);
        CreateChapterAudioFileConfiguration(modelBuilder);
        CreateAudiobookFileConfiguration(modelBuilder);
        CreateFileMetadataConfiguration(modelBuilder);
        CreateLibraryPathConfiguration(modelBuilder);
        CreateLibrarySettingsConfiguration(modelBuilder);
        CreateConversionJobConfiguration(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void CreateConversionJobConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConversionJob>().HasKey(cj => cj.Id);
    }

    private static void CreateLibraryPathConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LibraryPath>().HasKey(lp => lp.Id);
        modelBuilder.Entity<LibraryPath>().HasOne(lp => lp.LibrarySettings)
            .WithMany(ls => ls.Paths)
            .HasForeignKey(lp => lp.LibrarySettingsId);
    }

    private static void CreateLibrarySettingsConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LibrarySettings>().HasKey(ls => ls.Id);
        modelBuilder.Entity<LibrarySettings>().HasMany(ls => ls.Paths)
            .WithOne(lp => lp.LibrarySettings)
            .HasForeignKey(lp => lp.LibrarySettingsId);
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