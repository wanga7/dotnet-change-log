using DotNetChangelog.ConventionalCommit;
using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;

namespace DotNetChangelog.Tests.Utilities.ChangelogExtensions;

public class WhenGettingTitleForContinuousChangelog
{
    private const string ExpectedTitle = "1.1.0 (2020-10-15)";

    private static readonly Changelog Changelog =
        new(
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0",
                new DateTimeOffset(2020, 10, 14, 12, 0, 0, TimeSpan.Zero)
            ),
            new VersionTag(
                "dotnet-change-log@",
                "1.1.0",
                string.Empty,
                "dotnet-change-log@1.1.0",
                new DateTimeOffset(2020, 10, 15, 12, 0, 0, TimeSpan.Zero)
            ),
            new ConventionalCommits(
                Array.Empty<GitCommit>(),
                Array.Empty<GitCommit>(),
                Array.Empty<GitCommit>()
            )
        );

    private string _generatedTitle;

    [OneTimeSetUp]
    public void Context()
    {
        _generatedTitle = Changelog.GetTitleForContinuousChangelog();
    }

    [Test]
    public void ShouldIncludeToVersionTagAndDate()
    {
        Assert.That(_generatedTitle, Is.EqualTo(ExpectedTitle));
    }
}
