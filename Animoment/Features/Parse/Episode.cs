namespace Animoment.Features.Parse;

public readonly record struct Episode(string ShowName, int SeasonNumber, int EpisodeNumber, int OpenSubId, string Subtitles);
