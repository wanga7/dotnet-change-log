using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;
using LibGit2Sharp;

namespace DotNetChangelog.Utilities;

public static class GitTagExtensions
{
    // Matching a 3-number or 4-number version string, with an optional suffix starting with "-" or "_"
    private static readonly Regex VersionTagPattern =
        new(@"^(.+?)(\d+\.\d+\.\d+(\.\d+)?)(-.*|_.*)?$");

    public static Result<VersionTag> ToVersionTag(this Tag tag)
    {
        Match match = VersionTagPattern.Match(tag.FriendlyName);

        if (!match.Success)
        {
            return Result.Failure<VersionTag>($"cannot match regex \"{VersionTagPattern}\"");
        }

        string prefix = match.Groups[1].Value;
        string version = match.Groups[2].Value;
        string suffix = match.Groups[4].Value;

        return Result.Success(new VersionTag(prefix, version, suffix, tag.FriendlyName));
    }

    public static bool Matches(this Tag tag, string pattern)
    {
        return tag.FriendlyName == pattern || new Regex(pattern).Match(tag.FriendlyName).Success;
    }
}
