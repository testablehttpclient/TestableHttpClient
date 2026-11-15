namespace TestableHttpClient;

internal readonly struct Any;
internal readonly union HttpMethodOrAny(Any, HttpMethod);
internal readonly union VersionOrAny(Any, Version);
internal readonly union UriPatternValue(Any, ExactValue, PatternValue);
internal record struct PatternValue(string Pattern);
internal record struct ExactValue(string Value);
internal readonly union HeaderValue(Any, PatternValue);
internal sealed class HeaderList : Dictionary<string, HeaderValue> { }
internal readonly union Headers(Any, HeaderList);
internal readonly union Content(Any, PatternValue);
