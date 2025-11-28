using System.IO;
using System.Runtime.CompilerServices;

using TestableHttpClient.Utils;
using static TestableHttpClient.Utils.HttpRequestMessageFormatter;

namespace TestableHttpClient.Tests.Utils;

public sealed class HttpRequestMessageFormatterTests
{
    private static string FetchTestData(string filename, [CallerFilePath] string callerFilePath = "")
    {
        string directory = Path.GetDirectoryName(callerFilePath)!;
        string filePath = Path.Combine(directory, "HttpRequestMessageFormatterTestData", filename);
        filePath += ".verified.http";
        return File.ReadAllText(filePath);
    }

    [Fact]
    public void Format_NullRequest_CreatesExpectedString()
    {
        HttpRequestMessage? request = null;

        var result = Format(request, HttpRequestMessageFormatOptions.All);

        Assert.Equal("null", result);
    }

    [Theory]
    [InlineData("1.0")]
    [InlineData("1.1")]
    [InlineData("2.0")]
    [InlineData("3.0")]
    public void Format_SimpleGetRequest_CreatesExpectedString(string version)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com")
        {
            Version = Version.Parse(version)
        };
        string result = Format(request, HttpRequestMessageFormatOptions.All);

        string expected = FetchTestData($"simple_get_request_version_{version}");

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Format_SimplePostRequestWithHeadersAndBody_CreatesExpectedString()
    {
        using HttpRequestMessage request = new(HttpMethod.Post, "https://example.com")
        {
            Version = HttpVersion.Version11
        };
        request.Content = new StringContent("Hello, World!");
        request.Content.Headers.ContentLength = 13;
        string result = Format(request, HttpRequestMessageFormatOptions.All);

        string expected = FetchTestData("simple_post_request");

        Assert.Equal(expected, result);
    }

    [Fact]
    public void FormatRequestLineOptions_AnyRequest_CreatesExpectedString()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com") { Version = HttpVersion.Version11 };
        
        Assert.Multiple(
            () => Assert.Equal("GET", Format(request, HttpRequestMessageFormatOptions.HttpMethod)),
            () => Assert.Equal("https://example.com/", Format(request, HttpRequestMessageFormatOptions.RequestUri)),
            () => Assert.Equal("HTTP/1.1", Format(request, HttpRequestMessageFormatOptions.HttpVersion)),
            () => Assert.Equal("GET https://example.com/", Format(request, HttpRequestMessageFormatOptions.HttpMethod | HttpRequestMessageFormatOptions.RequestUri)),
            () => Assert.Equal("https://example.com/ HTTP/1.1", Format(request, HttpRequestMessageFormatOptions.RequestUri | HttpRequestMessageFormatOptions.HttpVersion)),
            () => Assert.Equal("GET https://example.com/ HTTP/1.1\r\n", Format(request, HttpRequestMessageFormatOptions.RequestLine))
        );
    }

    [Fact]
    public void FormatRequestLineHeaders_SimplePostRequestWithHeadersAndBody_CreatesExpectedString()
    {
        using HttpRequestMessage request = new(HttpMethod.Post, "https://example.com")
        {
            Version = HttpVersion.Version11
        };
        request.Content = new StringContent("Hello, World!");
        request.Content.Headers.ContentLength = 13;
        string result = Format(request, HttpRequestMessageFormatOptions.HttpMethod|HttpRequestMessageFormatOptions.RequestUri|HttpRequestMessageFormatOptions.Headers);

        string expected = FetchTestData("method_uri_headers_post_request");

        Assert.Equal(expected, result);
    }

    [Fact]
    public void FormatRequestLineContent_SimplePostRequestWithHeadersAndBody_CreatesExpectedString()
    {
        using HttpRequestMessage request = new(HttpMethod.Post, "https://example.com")
        {
            Version = HttpVersion.Version11
        };
        request.Content = new StringContent("Hello, World!");
        request.Content.Headers.ContentLength = 13;
        string result = Format(request, HttpRequestMessageFormatOptions.HttpMethod | HttpRequestMessageFormatOptions.RequestUri | HttpRequestMessageFormatOptions.Content);

        string expected = FetchTestData("method_uri_content_post_request");

        Assert.Equal(expected, result);
    }
}
