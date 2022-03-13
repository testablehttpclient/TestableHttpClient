namespace TestableHttpClient.NFluent.Tests;

public class FluentHttpRequestMessagesChecksTests
{
#nullable disable
    [Fact]
    public void Constructor_NullHttpRequestMessages_ThrowsArgumentNullException()
    {
        Check.ThatCode(() => new FluentHttpRequestMessagesChecks(null))
            .Throws<ArgumentNullException>()
            .WithProperty(x => x.ParamName, "httpRequestMessages");
    }
#nullable restore

#nullable disable
    [Fact]
    public void WithFilter_NullPredicate_Fails()
    {
        var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
        Check.ThatCode(() => sut.WithFilter(null, "check"))
            .IsAFailingCheckWithMessage("",
            "The request filter should not be null.");
    }
#nullable restore

    [Fact]
    public void WithFilter_PredicateThatDoesNotMatchAnyRequests_Fails()
    {
        var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        Check.ThatCode(() => sut.WithFilter(x => x == null, string.Empty))
            .IsAFailingCheckWithMessage("",
            "Expected at least one request to be made, but no requests were made.");
    }

    [Fact]
    public void WithFilter_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_FailsWithMessage()
    {
        var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        Check.ThatCode(() => sut.WithFilter(x => x == null, "custom check"))
            .IsAFailingCheckWithMessage("",
            "Expected at least one request to be made with custom check, but no requests were made.");
    }

    [Fact]
    public void WithFilter_PredicateThatDoesMatchAnyRequests_DoesNotFail()
    {
        var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        Check.ThatCode(() => sut.WithFilter(x => x != null, string.Empty)).Not.IsAFailingCheck();
    }

#nullable disable
    [Fact]
    public void WithFilter_WithRequestExpectations_NullPredicate_Fails()
    {
        var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
        Check.ThatCode(() => sut.WithFilter(null, 1, "check"))
            .IsAFailingCheckWithMessage("",
            "The request filter should not be null.");
    }
#nullable restore

    [Fact]
    public void WithFilter_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequests_Fails()
    {
        var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        Check.ThatCode(() => sut.WithFilter(x => x == null, 1, string.Empty))
            .IsAFailingCheckWithMessage("",
            "Expected one request to be made, but no requests were made.");
    }

    [Fact]
    public void WithFilter_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_FailsWithMessage()
    {
        var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        Check.ThatCode(() => sut.WithFilter(x => x == null, 1, "custom check"))
            .IsAFailingCheckWithMessage("",
            "Expected one request to be made with custom check, but no requests were made.");
    }

    [Fact]
    public void WithFilter_WithRequestExpectation_PredicateThatDoesMatchAnyRequests_DoesNotFail()
    {
        var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        Check.ThatCode(() => sut.WithFilter(x => x != null, 1, string.Empty)).Not.IsAFailingCheck();
    }
}
