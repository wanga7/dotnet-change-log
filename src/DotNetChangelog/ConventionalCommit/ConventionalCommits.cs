using DotNetChangelog.Domain;

namespace DotNetChangelog.ConventionalCommit;

/// <summary>
///     https://www.conventionalcommits.org/en/v1.0.0/#summary
/// </summary>
public record ConventionalCommits(
    GitCommit[] Features,
    GitCommit[] Fixes,
    GitCommit[] OtherNotableChanges
);
