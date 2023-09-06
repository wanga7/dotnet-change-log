using DotNetChangelog.Domain;

namespace DotNetChangelog.Tests.Domain.OfficialVersionComparerDesc;

[TestFixtureSource(nameof(TestCases))]
public class WhenComparing
{
    private readonly VersionTag _x;
    private readonly VersionTag _y;
    private readonly int _expectedComparisonResult;

    private int _actualComparisonResult;

    public WhenComparing(VersionTag x, VersionTag y, int expectedComparisonResult)
    {
        _x = x;
        _y = y;
        _expectedComparisonResult = expectedComparisonResult;
    }

    [OneTimeSetUp]
    public void Context()
    {
        _actualComparisonResult = new DotNetChangelog.Domain.OfficialVersionComparerDesc().Compare(
            _x,
            _y
        );
    }

    [Test]
    public void ShouldReturnExpectedComparisonResult()
    {
        Assert.That(_actualComparisonResult, Is.EqualTo(_expectedComparisonResult));
    }

    private static object[] TestCases =
    {
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            0
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            new VersionTag(
                "dotnet-change-log@",
                "1.1.0",
                string.Empty,
                "dotnet-change-log@1.1.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            1
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.1.0",
                string.Empty,
                "dotnet-change-log@1.1.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            -1
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0",
                "_alpha",
                "dotnet-change-log@1.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            0
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            0
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            new VersionTag(
                "dotnet-change-log@",
                "1.1.0.0",
                string.Empty,
                "dotnet-change-log@1.1.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            1
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.1.0.0",
                string.Empty,
                "dotnet-change-log@1.1.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            -1
        },
        new object[]
        {
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0.0",
                "_alpha",
                "dotnet-change-log@1.0.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            new VersionTag(
                "dotnet-change-log@",
                "1.0.0.0",
                string.Empty,
                "dotnet-change-log@1.0.0.0",
                new DateTimeOffset(2020, 10, 14, 10, 0, 0, TimeSpan.Zero)
            ),
            0
        },
    };
}
