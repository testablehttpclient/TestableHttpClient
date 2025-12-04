namespace TestableHttpClient.Utils;

[Flags]
internal enum RequestFormatOptions
{
    HttpMethod = 1,
    RequestUri = 2,
    HttpVersion = 4,
    RequestLine = HttpMethod | RequestUri | HttpVersion,
    Headers = 8,
    Content = 16,
    All = RequestLine | Headers | Content
}
