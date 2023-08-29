using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;
using LibGit2Sharp;

namespace DotNetChangelog;

public class GitHistory
{
    // Matching a 3-number or 4-number version string, with an optional suffix starting with "-" or "_"
    private static readonly Regex VersionTagPattern =
        new(@"^(.+?)(\d+\.\d+\.\d+(\.\d+)?)(-.*|_.*)?$");

    private static readonly IComparer<VersionTag> OfficialVersionComparerDesc =
        new OfficialVersionComparerDesc();

    private readonly IRepository _repo;

    public GitHistory(string repoPath)
    {
        _repo = new Repository(repoPath);
    }

    /// <summary>
    ///     Get change commits between two tags, excluding both ends, from newest to oldest
    /// </summary>
    public IReadOnlyList<GitCommit> GetHistoryDesc(string fromTag, string toTag)
    {
        if (
            _repo.Tags[fromTag].Target is not Commit fromCommit
            || _repo.Tags[toTag].Target is not Commit toCommit
        )
        {
            return Array.Empty<GitCommit>();
        }

        CommitFilter filter =
            new() { IncludeReachableFrom = toCommit, ExcludeReachableFrom = fromCommit };

        List<GitCommit> commits = new();

        // Filter out merge commits (where commit.Parents.Count() > 1)
        commits.AddRange(
            _repo.Commits
                .QueryBy(filter)
                .Where(c => c.Parents.Count() == 1)
                .Select(c => c.Convert())
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
        Result<VersionTag> fromTagVersionInfo = GetVersionTag(fromTagName);
        if (fromTagVersionInfo.IsFailure)
        {
            Console.WriteLine(
                $"Failed to extract version info from tag \"{fromTagName}\": {fromTagVersionInfo.Error}"
            );
            return Array.Empty<VersionTag>();
        }

        Result<VersionTag> toTagVersionInfo = GetVersionTag(toTagName);
        if (toTagVersionInfo.IsFailure)
        {
            Console.WriteLine(
                $"Failed to extract version info from tag \"{toTagName}\": {toTagVersionInfo.Error}"
            );
            return Array.Empty<VersionTag>();
        }

        SortedList<VersionTag, VersionTag> sortedTags = new(OfficialVersionComparerDesc);
        foreach (Tag tag in _repo.Tags)
        {
            if (!tag.Matches(pattern))
            {
                continue;
            }

            Result<VersionTag> versionTag = GetVersionTag(tag.FriendlyName);
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

    public Result<VersionTag> GetVersionTag(string tagName)
    {
        Tag tag = _repo.Tags[tagName];

        if (tag.Target is not Commit commit)
        {
            return Result.Failure<VersionTag>("failed to get commit from tag");
        }

        Match match = VersionTagPattern.Match(tagName);
        if (!match.Success)
        {
            return Result.Failure<VersionTag>($"cannot match regex \"{VersionTagPattern}\"");
        }

        string prefix = match.Groups[1].Value;
        string version = match.Groups[2].Value;
        string suffix = match.Groups[4].Value;

        return Result.Success(new VersionTag(prefix, version, suffix, tagName, commit.Author.When));
    }

    private static bool IsBetween(VersionTag subject, VersionTag fromVersion, VersionTag toVersion)
    {
        return OfficialVersionComparerDesc.Compare(subject, fromVersion) <= 0
            && OfficialVersionComparerDesc.Compare(subject, toVersion) >= 0;
    }
}
