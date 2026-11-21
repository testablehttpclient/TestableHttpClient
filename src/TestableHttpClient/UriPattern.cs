namespace TestableHttpClient;

internal readonly record struct UriPattern
{
    public UriPattern()
    {
        Scheme = new Any();
        Host = new Any();
        Port = new Any();
        Path = new Any();
        Query = new Any();
    }

    public UriPatternValue Scheme { get; init; }
    public UriPatternValue Host { get; init; }
    public UriPatternValue Port { get; init; }
    public UriPatternValue Path { get; init; }
    public UriPatternValue Query { get; init; }

    public bool IsAny => Scheme is Any && Host is Any && Port is Any && Path is Any && Query is Any;
}
