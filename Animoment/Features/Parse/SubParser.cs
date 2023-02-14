namespace Animoment.Features.Parse;

public class SubParser
{

    private static string CleanText(string text)
    {
        text = text.Replace("[^\\x00-\\x7F]", "");
        text = text.Replace("[\\p{Cntrl}&&[^\r\n\t]]", "");
        text = text.Replace("\\p{C}", "");

        return text.Trim();
    }

    private static SubTiming ParseTiming(string timing)
    {
        string[] split = timing.Split(':');
        int hours = int.Parse(split[0]);
        int minutes = int.Parse(split[1]);

        int seconds;
        int milliseconds;
        if (split[2].Contains(","))
        {
            seconds = int.Parse(split[2].Split(",")[0]);
            milliseconds = int.Parse(split[2].Split(",")[1]);
        }
        else
        {
            seconds = int.Parse(split[2].Split(".")[0]);
            milliseconds = int.Parse(split[2].Split(".")[1]);
        }

        return new SubTiming(hours, minutes, seconds, milliseconds);
    }

    public List<Subtitle> Parse(string[] text)
    {
        if (string.Join("\n", text).Contains("[Script Info]"))
        {
            return ParseSSA(text);
        }
        return ParseSRT(text);
    }

    private List<Subtitle> ParseSRT(string[] text)
    {
        int lineNum = 0;
        List<Subtitle> subtitles = new List<Subtitle>();


        while (!text[lineNum].Replace("\uFEFF", "").StartsWith("1"))
        {
            lineNum++;
        }

        while (lineNum < text.Length)
        {
            int subCount = int.Parse(text[lineNum].Replace("\uFEFF", "").Trim());
            lineNum++;

            string startTime = text[lineNum].Split("-->")[0].Trim();
            string endTime = text[lineNum].Split("-->")[1].Trim();
            lineNum++;

            string content = string.Empty;
            while (lineNum < text.Length && !text[lineNum].Replace("\uFEFF", "").StartsWith((subCount + 1).ToString()))
            {
                content += text[lineNum];
                lineNum++;
            }

            subtitles.Add(new Subtitle(subCount, ParseTiming(startTime), ParseTiming(endTime), content.Trim()));
        }

        return subtitles;
    }

    private List<Subtitle> ParseSSA(string[] text)
    {
        int lineNum = 0;
        List<Subtitle> subtitles = new List<Subtitle>();

        while (!text[lineNum].Replace("\uFEFF", "").StartsWith("Dialogue:"))
        {
            lineNum++;
        }

        int subCount = 1;
        while (lineNum < text.Length && text[lineNum].Contains("Dialogue:"))
        {
            string[] split = text[lineNum].Split(",");
            string startTime = split[1].Trim();
            string endTime = split[2].Trim();
            string content = split[^1].Trim();

            subtitles.Add(new Subtitle(subCount, ParseTiming(startTime), ParseTiming(endTime), content));
            lineNum++;
            subCount++;
        }

        return subtitles;
    }
}
