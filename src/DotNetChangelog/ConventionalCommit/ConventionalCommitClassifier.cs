using DotNetChangelog.Domain;

namespace DotNetChangelog.ConventionalCommit;

/// <summary>
///     https://www.conventionalcommits.org/en/v1.0.0/#summary
/// </summary>
public static class ConventionalCommitClassifier
{
    public static ConventionalCommits Classify(IReadOnlyList<GitCommit> commits)
    {
        List<GitCommit> features = new();
        List<GitCommit> fixes = new();
        List<GitCommit> otherNotableChanges = new();

        foreach (GitCommit commit in commits)
        {
            if (IsFeature(commit))
            {
                features.Add(commit);
            }
            else if (IsFix(commit))
            {
                fixes.Add(commit);
            }
            else if (IsNotableChange(commit))
            {
                otherNotableChanges.Add(commit);
            }
        }

        return new(features.ToArray(), fixes.ToArray(), otherNotableChanges.ToArray());
    }

    private static bool IsFeature(GitCommit commit)
    {
        return DoesCommitMessageStartsWithAny(commit, "feat");
    }

    private static bool IsFix(GitCommit commit)
    {
        return DoesCommitMessageStartsWithAny(commit, "fix", "hotfix", "bugfix");
    }

    private static bool IsNotableChange(GitCommit commit)
    {
        return !DoesCommitMessageStartsWithAny(commit, "chore", "build", "release", "ci", "style");
    }

    private static bool DoesCommitMessageStartsWithAny(GitCommit commit, params string[] prefixes)
    {
        return prefixes.Any(
            prefix => commit.ShortMessage.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
        );
    }
}
