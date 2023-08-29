using DotNetChangelog.Domain;

namespace DotNetChangelog.Utilities;

public static class ChangelogExtensions
{
    public static string GetTitleForDirectChangelog(this Changelog changelog) =>
        $"{changelog.FromTag.GetTitleFromVersionTag()}...{changelog.ToTag.GetTitleFromVersionTag()}";

    public static string GetTitleForContinuousChangelog(this Changelog changelog) =>
        changelog.ToTag.GetTitleFromVersionTag();

    private static string GetTitleFromVersionTag(this VersionTag versionTag) =>
        $"{versionTag.GetVersionInfo()} ({versionTag.UtcTime:yyyy-MM-dd})";
}
