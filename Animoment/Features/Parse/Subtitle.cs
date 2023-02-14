namespace Animoment.Features.Parse;

public readonly record struct Subtitle(int Index, SubTiming StartTime, SubTiming EndTime,  string Content);