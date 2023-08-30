using CSharpFunctionalExtensions;
using DotNetChangelog.ConventionalCommit;
using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;

namespace DotNetChangelog.IO;

public class TextWriter : ChangelogWriter
{
    public TextWriter(string repoDirectory, string outputDirectory)
        : base(repoDirectory, outputDirectory) { }

    public override Result<string> Write(Changelog changelog) => PrependToFile(GetLines(changelog));

    public override Result<string> Write(ContinuousChangelog continuousChangelog) =>
        PrependToFile(GetLines(continuousChangelog));

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

    protected static Result<string> PrependToFile(string file, string text)
    {
        try
        {
            string existingLines = File.Exists(file) ? File.ReadAllText(file) : string.Empty;
            File.WriteAllText(file, text + Environment.NewLine + existingLines);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ex.ToString());
        }

        return Result.Success(Path.GetFullPath(file));
    }

    private Result<string> PrependToFile(IReadOnlyList<string> lines)
    {
        return PrependToFile(
            Path.Combine(_outputDirectory, FileName + ".txt"),
            string.Join(Environment.NewLine, lines)
        );
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
