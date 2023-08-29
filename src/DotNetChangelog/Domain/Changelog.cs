using DotNetChangelog.ConventionalCommit;

namespace DotNetChangelog.Domain;

public record Changelog(
    VersionTag FromTag,
    VersionTag ToTag,
    ConventionalCommits ConventionalCommits
);

public record ContinuousChangelog(IReadOnlyList<Changelog> SortedChangelogs);
