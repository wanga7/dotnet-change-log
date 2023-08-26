namespace DotNetChangelog.Domain;

public record Changelog(VersionTag FromTag, VersionTag ToTag, IReadOnlyList<GitCommit> Commits);

public record ContinuousChangelog(IReadOnlyList<Changelog> SortedChangelogs);
