using MovieCollection.OpenSubtitles;
using MovieCollection.OpenSubtitles.Models;

namespace Animoment.Features.Parse;

public class SubDownloader
{

    private readonly OpenSubtitlesService _openSubtitlesService;

    public SubDownloader(OpenSubtitlesService openSubtitlesService)
    {
        _openSubtitlesService = openSubtitlesService;
    }

    public async void Search(string imdbId)
    {
        var search = new NewSubtitleSearch
        {
            ParentImdbId = int.Parse(imdbId)
        };
        
        var results = (await _openSubtitlesService.SearchSubtitlesAsync(search)).Data;
        IEnumerable<int> fileIds = results[0].Attributes.Files.Select((file) => file.FileId);
        
        Console.WriteLine(fileIds);
    }
}