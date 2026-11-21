namespace TestableHttpClient;

internal readonly struct Any;
internal readonly struct None;
internal readonly union HttpMethodOrAny(Any, HttpMethod);
internal readonly union VersionOrAny(Any, Version);
internal readonly union UriPatternValue(Any, ExactValue, PatternValue);
internal record struct PatternValue(string Pattern);
internal record struct ExactValue(string Value);
internal readonly union HeaderValue(Any, PatternValue);
internal sealed class HeaderList : Dictionary<string, HeaderValue> { }
internal readonly union Headers(None, Any, HeaderList);
internal readonly union Content(None, Any, PatternValue);
