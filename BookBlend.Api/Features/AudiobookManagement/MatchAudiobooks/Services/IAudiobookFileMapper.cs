using BookBlend.Api.Entities;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Services;

public interface IAudiobookFileMapper
{
    Audiobook MapAudiobookFileToAudiobook(AudiobookFile audiobookFile, string chapterName, string defaultLanguage);
    Chapter MapAudiobookFileToChapter(AudiobookFile audiobookFile);
}