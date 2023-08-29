using CSharpFunctionalExtensions;
using DotNetChangelog.ConventionalCommit;
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
        lines.AddRange(GetLines(changelog.ConventionalCommits));
        return lines;
    }

    protected static IReadOnlyList<string> GetLines(ContinuousChangelog continuousChangelog)
    {
        List<string> lines = new();

        foreach (Changelog changelog in continuousChangelog.SortedChangelogs)
        {
            lines.Add(changelog.GetTitleForContinuousChangelog());
            lines.AddRange(GetLines(changelog.ConventionalCommits));
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

    private static IReadOnlyList<string> GetLines(ConventionalCommits commits)
    {
        List<string> lines = new();

        if (commits.Features.Length > 0)
        {
            lines.Add(ConventionalCommitCategory.Feature);
            lines.AddRange(commits.Features.Select(c => c.Format()));
        }

        if (commits.Fixes.Length > 0)
        {
            lines.Add(ConventionalCommitCategory.Fixes);
            lines.AddRange(commits.Fixes.Select(c => c.Format()));
        }

        if (commits.OtherNotableChanges.Length > 0)
        {
            lines.Add(ConventionalCommitCategory.OtherNotableChanges);
            lines.AddRange(commits.OtherNotableChanges.Select(c => c.Format()));
        }

        return lines.Count > 0 ? lines : new[] { ConventionalCommitCategory.NoNotableChanges };
    }
}
