using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using TestableHttpClient.Utils;

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
    public void HttpRequestMessage_Debug_Test()
    {
        TestContext.Current.TestOutputHelper!.WriteLine(RuntimeInformation.FrameworkDescription);
        TestContext.Current.TestOutputHelper.WriteLine($"OS: {RuntimeInformation.OSDescription}");
        TestContext.Current.TestOutputHelper.WriteLine($"CLR/Environment version: {Environment.Version}");
        TestContext.Current.TestOutputHelper.WriteLine($"Assembly: {typeof(HttpRequestMessage).Assembly.Location}");
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        TestContext.Current.TestOutputHelper.WriteLine($"Default version: {request.Version}");
    }

    [Fact]
    public void Format_NullRequest_CreatesExpectedString()
    {
        HttpRequestMessage? request = null;

        var result = HttpRequestMessageFormatter.Format(request, HttpRequestMessageFormatOptions.All);

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
        string result = HttpRequestMessageFormatter.Format(request, HttpRequestMessageFormatOptions.All);

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
        string result = HttpRequestMessageFormatter.Format(request, HttpRequestMessageFormatOptions.All);

        string expected = FetchTestData("simple_post_request");

        Assert.Equal(expected, result);
    }
}
