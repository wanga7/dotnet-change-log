using DotNetChangelog.Domain;

namespace DotNetChangelog.Tests.Domain.VersionTagExtensions;

[TestFixtureSource(nameof(TestCases))]
public class WhenCheckingForPreRelease
{
    private readonly VersionTag _versionTag;
    private readonly bool _shouldBePreRelease;

    private bool _checkResult;

    public WhenCheckingForPreRelease(VersionTag versionTag, bool shouldBePreRelease)
    {
        _versionTag = versionTag;
        _shouldBePreRelease = shouldBePreRelease;
    }

    [OneTimeSetUp]
    public void Context()
    {
        _checkResult = _versionTag.IsPreRelease();
    }

    [Test]
    public void ShouldReturnExpectedCheckResult()
    {
        Assert.That(_checkResult, Is.EqualTo(_shouldBePreRelease));
    }

    private static readonly object[] TestCases =
    {
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0",
                DateTimeOffset.MinValue
            ),
            false
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0",
                "alpha",
                "dotnet-change-log@1.0.0_alpha",
                DateTimeOffset.MinValue
            ),
            true
        },
    };
}
