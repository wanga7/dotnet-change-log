using System.Text.RegularExpressions;
using LibGit2Sharp;

namespace DotNetChangelog.Utilities;

public static class GitTagExtensions
{
    public static bool Matches(this Tag tag, string pattern)
    {
        return tag.FriendlyName == pattern || new Regex(pattern).Match(tag.FriendlyName).Success;
    }
}
