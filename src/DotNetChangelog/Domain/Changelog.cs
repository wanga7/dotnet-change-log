namespace DotNetChangelog.Domain;

public record Changelog(string FromTag, string ToTag, IReadOnlyList<GitCommit> Commits)
{
    public override string ToString()
    {
        return $"{FromTag}...{ToTag}: {Commits.Count} commits";
    }
}
