namespace DotNetChangelog;

public record GitCommit(string Hash, string ShortMessage, string ParentHash)
{
    public override string ToString()
    {
        return $"{Hash} {ShortMessage}";
    }
}
