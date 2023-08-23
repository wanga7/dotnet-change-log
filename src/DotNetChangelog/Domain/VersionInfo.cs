namespace DotNetChangelog.Domain;

public record VersionInfo(string Prefix, string Version, string Suffix);

/// <summary>
///     Only compares by 'Version', ignoring 'Prefix' and 'Suffix'
/// </summary>
public class OfficialVersionComparerDesc : IComparer<VersionInfo>
{
    public int Compare(VersionInfo x, VersionInfo y)
    {
        Version versionX = new(x.Version);
        Version versionY = new(y.Version);

        return versionY.CompareTo(versionX);
    }
}

public static class VersionInfoExtensions
{
    public static bool IsPreRelease(this VersionInfo versionInfo) =>
        !string.IsNullOrEmpty(versionInfo.Suffix);
}
