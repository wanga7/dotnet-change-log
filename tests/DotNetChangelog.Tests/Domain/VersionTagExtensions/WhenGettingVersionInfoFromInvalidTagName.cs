using DotNetChangelog.Domain;

namespace DotNetChangelog.Tests.Domain.VersionTagExtensions;

public class WhenGettingVersionInfoFromInvalidTagName
{
    private const string ExpectedVersionInfo = "1.0.0 alpha";

    private static readonly VersionTag Tag =
        new("dotnet-change-log@", "1.0.0", "alpha", "invalid tag name", DateTimeOffset.MinValue);

    private string _versionInfo;

    [OneTimeSetUp]
    public void Context()
    {
        _versionInfo = Tag.GetVersionInfo();
    }

    [Test]
    public void ShouldFallbackToExplicitFields()
    {
        Assert.That(_versionInfo, Is.EqualTo(ExpectedVersionInfo));
    }
}
