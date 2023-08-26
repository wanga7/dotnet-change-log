using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;

namespace DotNetChangelog.IO;

public class TextWriter : ChangelogWriter
{
    public TextWriter(string repoDirectory, string outputDirectory)
        : base(repoDirectory, outputDirectory) { }

    public override Result<string> Write(Changelog changelog) => WriteToFile(GetLines(changelog));

    public override Result<string> Write(ContinuousChangelog continuousChangelog) =>
        WriteToFile(GetLines(continuousChangelog));

    protected static IReadOnlyList<string> GetLines(Changelog changelog)
    {
        List<string> lines = new() { changelog.GetTitleForDirectChangelog() };
        lines.AddRange(changelog.Commits.Select(c => c.Format()));
        return lines;
    }

    protected static IReadOnlyList<string> GetLines(ContinuousChangelog continuousChangelog)
    {
        List<string> lines = new();

        foreach (Changelog changelog in continuousChangelog.SortedChangelogs)
        {
            lines.Add(changelog.GetTitleForContinuousChangelog());
            lines.AddRange(changelog.Commits.Select(commit => commit.Format()));
            lines.Add(string.Empty);
        }

        return lines;
    }

    private Result<string> WriteToFile(IReadOnlyList<string> lines)
    {
        string file = Path.Combine(_outputDirectory, FileName + ".txt");

        try
        {
            File.WriteAllText(file, string.Join(Environment.NewLine, lines));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ex.ToString());
        }

        return Result.Success(Path.GetFullPath(file));
    }
}
