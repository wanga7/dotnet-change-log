using DotNetChangelog.Domain;

namespace DotNetChangelog.Tests.Domain.VersionTagExtensions;

public class WhenGettingVersionInfoFromTagWithoutSuffix
{
    private const string ExpectedVersionInfo = "1.0.0";

    private static readonly VersionTag Tag =
        new(
            "dotnet-change-log@",
            "1.0.0",
            string.Empty,
            "dotnet-change-log@1.0.0",
            DateTimeOffset.MinValue
        );

    private string _versionInfo;

    [OneTimeSetUp]
    public void Context()
    {
        _versionInfo = Tag.GetVersionInfo();
    }

    [Test]
    public void ShouldGetExpectedVersionInfo()
    {
        Assert.That(_versionInfo, Is.EqualTo(ExpectedVersionInfo));
    }
}
