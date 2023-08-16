using DotNetChangelog.Domain;

namespace DotNetChangelog.Utilities;

public static class ChangelogExtensions
{
    public static string GetSummary(this Changelog changelog) =>
        $"{changelog.FromTag}...{changelog.ToTag}";
}
