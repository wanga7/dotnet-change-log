namespace DotNetChangelog.Domain;

public enum ChangelogMode
{
    /// <summary>
    ///     Generate a changelog based on the diff between just the start and end tag
    /// </summary>
    Direct,

    /// <summary>
    ///     Generate a changelog detailing changes on every tag between start and end tag, excluding pre-releases
    /// </summary>
    Continuous
}
