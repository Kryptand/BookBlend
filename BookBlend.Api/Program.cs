using BookBlend.Api.Database;
using BookBlend.Api.Extensions;
using BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;
using BookBlend.Api.Features.FileManagement.FileSystemScanner.Services;
using BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Services;
using BookBlend.Api.Middleware;
using BookBlend.Api.Shared;
using Carter;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AudiobookDbContext>(options =>
{
    if (builder.Environment.IsEnvironment("Test"))
    {
        options.UseInMemoryDatabase("AudiobookDb");
        return;
    }

    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));



// Register your services
builder.Services.AddTransient<IFileSystemWrapper, FileSystemWrapper>();
builder.Services.AddTransient<ILibraryPathValidatorService, LibraryPathValidatorService>();
builder.Services.AddTransient<IFileScannerService, FileScannerService>();
builder.Services.AddTransient<IMapFileToAudiobookFile, MapFileToAudiobookFile>();
builder.Services.AddTransient<IAudiobookFilesMatcherService, AudiobookFilesMatcherService>();
builder.Services.AddTransient<IAudiobookFileMapper, AudiobookFileMapper>();
builder.Services.AddTransient<IAudiobookCreatorFactory, AudiobookCreatorFactory>();
builder.Services.AddTransient<IAudiobookEnricherService, AudiobookEnricherService>();
builder.Services.AddTransient<AudiobookTitleHelper>();
builder.Services.AddTransient<FileNameHelper>();



builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.ApplyMigrations();
}

app.MapCarter();

app.Run();
