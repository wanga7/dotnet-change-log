using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;
using LibGit2Sharp;

namespace DotNetChangelog;

public class GitHistory
{
    private static readonly IComparer<VersionTag> OfficialVersionComparerDesc =
        new OfficialVersionComparerDesc();

    private readonly string _repoPath;

    public GitHistory(string repoPath)
    {
        _repoPath = repoPath;
    }

    /// <summary>
    ///     Get change commits between two tags, excluding both ends, from newest to oldest
    /// </summary>
    public IReadOnlyList<GitCommit> GetHistoryDesc(string fromTag, string toTag)
    {
        using Repository repo = new(_repoPath);

        if (
            repo.Tags[fromTag].Target is not Commit fromCommit
            || repo.Tags[toTag].Target is not Commit toCommit
        )
        {
            return Array.Empty<GitCommit>();
        }

        CommitFilter filter =
            new() { IncludeReachableFrom = toCommit, ExcludeReachableFrom = fromCommit };

        List<GitCommit> commits = new();

        // Filter out merge commits (where commit.Parents.Count() > 1)
        commits.AddRange(
            repo.Commits.QueryBy(filter).Where(c => c.Parents.Count() == 1).Select(c => c.Convert())
        );

        commits.RemoveAt(0);

        return commits;
    }

    public IReadOnlyList<VersionTag> GetVersionTagsDesc(
        string pattern,
        string fromTagName,
        string toTagName
    )
    {
        using Repository repo = new(_repoPath);
        TagCollection tags = repo.Tags;

        Tag fromTag = tags[fromTagName];
        Tag toTag = tags[toTagName];
        Result<VersionTag> fromTagVersionInfo = fromTag.ToVersionTag();
        Result<VersionTag> toTagVersionInfo = toTag.ToVersionTag();

        if (fromTagVersionInfo.IsFailure)
        {
            Console.WriteLine(
                $"Failed to extract version info from tag \"{fromTag.FriendlyName}\": {fromTagVersionInfo.Error}"
            );
            return Array.Empty<VersionTag>();
        }

        if (toTagVersionInfo.IsFailure)
        {
            Console.WriteLine(
                $"Failed to extract version info from tag \"{toTag.FriendlyName}\": {toTagVersionInfo.Error}"
            );
            return Array.Empty<VersionTag>();
        }

        SortedList<VersionTag, VersionTag> sortedTags = new(OfficialVersionComparerDesc);
        foreach (Tag tag in tags)
        {
            if (!tag.Matches(pattern))
            {
                continue;
            }

            Result<VersionTag> versionTag = tag.ToVersionTag();
            if (versionTag.IsFailure)
            {
                Console.WriteLine(
                    $"Failed to extract version from tag \"{tag.FriendlyName}\": {versionTag.Error}"
                );
                continue;
            }

            if (versionTag.Value.IsPreRelease())
            {
                continue;
            }

            if (IsBetween(versionTag.Value, fromTagVersionInfo.Value, toTagVersionInfo.Value))
            {
                sortedTags.Add(versionTag.Value, versionTag.Value);
            }
        }

        return sortedTags.Select(t => t.Value).ToArray();
    }

    private static bool IsBetween(VersionTag subject, VersionTag fromVersion, VersionTag toVersion)
    {
        return OfficialVersionComparerDesc.Compare(subject, fromVersion) <= 0
            && OfficialVersionComparerDesc.Compare(subject, toVersion) >= 0;
    }
}
