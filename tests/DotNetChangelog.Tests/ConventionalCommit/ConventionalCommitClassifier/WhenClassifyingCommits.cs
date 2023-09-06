using DotNetChangelog.ConventionalCommit;
using DotNetChangelog.Domain;
using Classifier = DotNetChangelog.ConventionalCommit.ConventionalCommitClassifier;

namespace DotNetChangelog.Tests.ConventionalCommit.ConventionalCommitClassifier;

public class WhenClassifyingCommits
{
    private static readonly GitCommit[] Commits =
    {
        new()
        {
            Hash = "12345aaaaa",
            ParentHash = "12345bbbbb",
            ShortMessage = "feat: add cancel button"
        },
        new()
        {
            Hash = "12345ccccc",
            ParentHash = "12345ddddd",
            ShortMessage = "fix: correct cancel button"
        },
        new()
        {
            Hash = "12345eeeee",
            ParentHash = "12345fffff",
            ShortMessage = "hotfix: add cancel button"
        },
        new()
        {
            Hash = "12345ggggg",
            ParentHash = "12345hhhhh",
            ShortMessage = "bugfix: add cancel button"
        },
        new()
        {
            Hash = "12345iiiii",
            ParentHash = "12345jjjjj",
            ShortMessage = "perf: speed up cancel button"
        },
        new()
        {
            Hash = "12345kkkkk",
            ParentHash = "12345lllll",
            ShortMessage = "chore: add cancel button"
        },
        new()
        {
            Hash = "12345kkkkk",
            ParentHash = "12345lllll",
            ShortMessage = "build: add cancel button"
        },
        new()
        {
            Hash = "12345kkkkk",
            ParentHash = "12345lllll",
            ShortMessage = "release: add cancel button"
        },
        new()
        {
            Hash = "12345kkkkk",
            ParentHash = "12345lllll",
            ShortMessage = "ci: add cancel button"
        },
        new()
        {
            Hash = "12345kkkkk",
            ParentHash = "12345lllll",
            ShortMessage = "style: add cancel button"
        }
    };

    private static readonly ConventionalCommits ExpectedConventionalCommit =
        new(
            new[]
            {
                new GitCommit
                {
                    Hash = "12345aaaaa",
                    ParentHash = "12345bbbbb",
                    ShortMessage = "feat: add cancel button"
                }
            },
            new[]
            {
                new GitCommit
                {
                    Hash = "12345ccccc",
                    ParentHash = "12345ddddd",
                    ShortMessage = "fix: correct cancel button"
                },
                new GitCommit
                {
                    Hash = "12345eeeee",
                    ParentHash = "12345fffff",
                    ShortMessage = "hotfix: add cancel button"
                },
                new GitCommit
                {
                    Hash = "12345ggggg",
                    ParentHash = "12345hhhhh",
                    ShortMessage = "bugfix: add cancel button"
                },
            },
            new[]
            {
                new GitCommit
                {
                    Hash = "12345iiiii",
                    ParentHash = "12345jjjjj",
                    ShortMessage = "perf: speed up cancel button"
                },
            }
        );

    private ConventionalCommits _generatedCommits;

    [OneTimeSetUp]
    public void Context()
    {
        _generatedCommits = Classifier.Classify(Commits);
    }

    [Test]
    public void ShouldClassifyNotableCommits()
    {
        Assert.Multiple(() =>
        {
            Assert.That(
                _generatedCommits.Features,
                Is.EqualTo(ExpectedConventionalCommit.Features)
            );
            Assert.That(_generatedCommits.Fixes, Is.EqualTo(ExpectedConventionalCommit.Fixes));
            Assert.That(
                _generatedCommits.OtherNotableChanges,
                Is.EqualTo(ExpectedConventionalCommit.OtherNotableChanges)
            );
        });
    }
}
