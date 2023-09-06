using DotNetChangelog.Domain;

namespace DotNetChangelog.Tests.Domain.VersionTagExtensions;

[TestFixtureSource(nameof(TestCases))]
public class WhenCheckingForPatchVersion
{
    private readonly VersionTag _versionTag;
    private readonly bool _shouldBePatchVersion;

    private bool _checkResult;

    public WhenCheckingForPatchVersion(VersionTag versionTag, bool shouldBePatchVersion)
    {
        _versionTag = versionTag;
        _shouldBePatchVersion = shouldBePatchVersion;
    }

    [OneTimeSetUp]
    public void Context()
    {
        _checkResult = _versionTag.IsPatchVersion();
    }

    [Test]
    public void ShouldReturnExpectedCheckResult()
    {
        Assert.That(_checkResult, Is.EqualTo(_shouldBePatchVersion));
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
                "1.1.0",
                string.Empty,
                "dotnet-change-log@1.1.0",
                DateTimeOffset.MinValue
            ),
            false
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
            true
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
            false
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
            false
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
            true
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
            true
        },
    };
}
