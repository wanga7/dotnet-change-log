using DotNetChangelog.Domain;

namespace DotNetChangelog.Tests.Domain.VersionTagExtensions;

[TestFixtureSource(nameof(TestCases))]
public class WhenCheckingForNormalVersion
{
    private readonly VersionTag _versionTag;
    private readonly bool _shouldBeNormalVersion;

    private bool _checkResult;

    public WhenCheckingForNormalVersion(VersionTag versionTag, bool shouldBeNormalVersion)
    {
        _versionTag = versionTag;
        _shouldBeNormalVersion = shouldBeNormalVersion;
    }

    [OneTimeSetUp]
    public void Context()
    {
        _checkResult = _versionTag.IsNormalVersion();
    }

    [Test]
    public void ShouldReturnExpectedCheckResult()
    {
        Assert.That(_checkResult, Is.EqualTo(_shouldBeNormalVersion));
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
            true
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.1.0",
                string.Empty,
                "dotnet-change-log@1.1.0",
                DateTimeOffset.MinValue
            ),
            true
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.1",
                string.Empty,
                "dotnet-change-log@1.0.1",
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
            false
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0.0",
                DateTimeOffset.MinValue
            ),
            true
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.1.0.0",
                string.Empty,
                "dotnet-change-log@1.1.0.0",
                DateTimeOffset.MinValue
            ),
            true
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.1.1.0",
                string.Empty,
                "dotnet-change-log@1.1.1.0",
                DateTimeOffset.MinValue
            ),
            false
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.1.0.1",
                string.Empty,
                "dotnet-change-log@1.1.0.1",
                DateTimeOffset.MinValue
            ),
            false
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0.0",
                "alpha",
                "dotnet-change-log@1.0.0.0_alpha",
                DateTimeOffset.MinValue
            ),
            false
        },
    };
}
