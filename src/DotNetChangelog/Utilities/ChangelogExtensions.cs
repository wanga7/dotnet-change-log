using DotNetChangelog.Domain;

namespace DotNetChangelog.Utilities;

public static class ChangelogExtensions
{
    public static string GetTitleForDirectChangelog(this Changelog changelog) =>
        $"{changelog.FromTag.GetVersionInfo()}...{changelog.ToTag.GetVersionInfo()}";

    public static string GetTitleForContinuousChangelog(this Changelog changelog) =>
        changelog.ToTag.GetVersionInfo();
}
