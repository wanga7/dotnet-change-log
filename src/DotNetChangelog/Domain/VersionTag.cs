namespace DotNetChangelog.Domain;

public record VersionTag(string Prefix, string Version, string Suffix, string TagName);

/// <summary>
///     Only compares by 'Version', ignoring 'Prefix' and 'Suffix'
/// </summary>
public class OfficialVersionComparer : IComparer<VersionTag>
{
    public int Compare(VersionTag x, VersionTag y)
    {
        Version versionX = new(x.Version);
        Version versionY = new(y.Version);

        return versionX.CompareTo(versionY);
    }
}

/// <summary>
///     Only compares by 'Version', ignoring 'Prefix' and 'Suffix'
/// </summary>
public class OfficialVersionComparerDesc : IComparer<VersionTag>
{
    public int Compare(VersionTag x, VersionTag y)
    {
        Version versionX = new(x.Version);
        Version versionY = new(y.Version);

        return versionY.CompareTo(versionX);
    }
}

public static class VersionTagExtensions
{
    public static bool IsPreRelease(this VersionTag versionTag) =>
        !string.IsNullOrEmpty(versionTag.Suffix);

    public static bool IsPatchVersion(this VersionTag versionTag)
    {
        Version version = new(versionTag.Version);
        return version.Build != 0 || version.Revision != 0;
    }

    public static bool BelongsToTheSameMinorVersion(this VersionTag lhs, VersionTag rhs)
    {
        Version lhsVersion = new(lhs.Version);
        Version rhsVersion = new(rhs.Version);

        return lhsVersion.Major == rhsVersion.Major && lhsVersion.Minor == rhsVersion.Minor;
    }
}
