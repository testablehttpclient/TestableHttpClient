using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class MessageBuilderTests
{
    [Fact]
    public void BuildMessage_NoExpectedCountZeroActualCountDefaultExpectedRequest()
    {
        Request request = new(new());
        var result = MessageBuilder.BuildMessage(null, 0, request, []);

        Assert.Equal("Expected at least one request to be made, but no requests were made.", result);
    }

    [Theory]
    [InlineData(1, "one request was made")]
    [InlineData(2, "2 requests were made")]
    [InlineData(10, "10 requests were made")]
    public void BuildMessage_NoExpectedCountVariableActualCountDefaultExpectedRequest(int actualCount, string expectedMessage)
    {
        Request request = new(new());
        var result = MessageBuilder.BuildMessage(null, actualCount, request, []);

        Assert.Equal($"Expected at least one request to be made, and {expectedMessage}.", result);
    }

    [Fact]
    public void BuildMessage_ZeroExpectedCountZeroActualCountDefaultExpectedRequest()
    {
        Request request = new(new());
        var result = MessageBuilder.BuildMessage(0, 0, request, []);

        Assert.Equal($"Expected no requests to be made, and no requests were made.", result);
    }

    [Theory]
    [InlineData(1, "one request")]
    [InlineData(2, "2 requests")]
    [InlineData(10, "10 requests")]
    public void BuildMessage_VariableExpectedCountZeroActualCountDefaultExpectedRequest(int expectedCount, string expectedMessage)
    {
        Request request = new(new());
        var result = MessageBuilder.BuildMessage(expectedCount, 0, request, []);

        Assert.Equal($"Expected {expectedMessage} to be made, but no requests were made.", result);
    }

    [Theory]
    [InlineData(null, "at least one GET request")]
    [InlineData(1, "one GET request")]
    [InlineData(2, "2 GET requests")]
    [InlineData(10, "10 GET requests")]
    public void BuildMessage_VariableExpectedCountZeroActualCountRequestSpecifyingMethod(int? expectedCount, string expectedMessage)
    {
        Request request = new(new())
        {
            Method = HttpMethod.Get,
        };

        var result = MessageBuilder.BuildMessage(expectedCount, 0, request, []);

        Assert.Equal($"Expected {expectedMessage} to be made, but no requests were made.", result);
    }

    [Theory]
    [InlineData("/test", "/test")]
    [InlineData("https://*/test", "https://<any host>/test")]
    [InlineData("/test?Hello", "/test?Hello")]
    public void BuildMessage_OnexpectedCountZeroActualCountRequestSpecifyingUri(string uri, string expectedRepresentation)
    {
        Request request = new(new())
        {
            RequestUri = UriPatternParser.Parse(uri)
        };

        var result = MessageBuilder.BuildMessage(1, 0, request, []);

        Assert.Equal($"Expected one request to be made to '{expectedRepresentation}', but no requests were made.", result);
    }

    [Fact]
    public void BuildMessage_OnexpectedCountZeroActualCountRequestSpecifyingHeaders()
    {
        Request request = new(new())
        {
            Headers = new Dictionary<string, Value>()
            {
                ["Header1"] = Value.Any(),
                ["Header2"] = Value.Pattern("Value2"),
                ["Header3"] = Value.Exact("Value3"),
            }
        };

        var result = MessageBuilder.BuildMessage(1, 0, request, []);

        Assert.Equal("""
            Expected one request to be made with headers:
              Header1: <any value>
              Header2: 'Value2'
              Header3: 'Value3'
            , but no requests were made.
            """, result);
    }

    [Fact]
    public void BuildMessage_OnexpectedCountZeroActualCountRequestSpecifyingContent()
    {
        Request request = new(new())
        {
            Content = "test"
        };

        var result = MessageBuilder.BuildMessage(1, 0, request, []);

        Assert.Equal("""
            Expected one request to be made with content:
              test
            , but no requests were made.
            """, result);
    }

    [Fact]
    public void BuildMessage_OnexpectedCountZeroActualCountRequestSpecifyingHeadersSpecifyingContent()
    {
        Request request = new(new())
        {
            Headers = new Dictionary<string, Value>()
            {
                ["Content-Type"] = Value.Exact("application/json")
            },
            Content = """
            {
              "test": "value"
            }
            """
        };

        var result = MessageBuilder.BuildMessage(1, 0, request, []);

        Assert.Equal("""
            Expected one request to be made with headers:
              Content-Type: 'application/json'
            and content:
              {
                "test": "value"
              }
            , but no requests were made.
            """, result);
    }

    [Theory]
    [InlineData("with condition 1", "condition 1")]
    [InlineData("with condition 1, condition 2", "condition 1", "condition 2")]
    [InlineData("with condition 1, condition 2, condition 3", "condition 1", "condition 2", "condition 3")]
    public void BuildMessage_NullExpectedCountZeroActualCountVariableAmountOfConditions(string expectedCondition, params string[] conditions)
    {
        Request request = new(new());
        var result = MessageBuilder.BuildMessage(null, 0, request, conditions);

        Assert.Equal($"Expected at least one request to be made {expectedCondition}, but no requests were made.", result);
    }

    [Theory]
    [InlineData("with condition 1", "condition 1")]
    [InlineData("with condition 1, condition 2", "condition 1", "condition 2")]
    [InlineData("with condition 1, condition 2, condition 3", "condition 1", "condition 2", "condition 3")]
    public void BuildMessage_NullExpectedCountOneActualCountVariableAmountOfConditions(string expectedCondition, params string[] conditions)
    {
        Request request = new(new());
        var result = MessageBuilder.BuildMessage(null, 1, request, conditions);

        Assert.Equal($"Expected at least one request to be made {expectedCondition}, and one request was made.", result);
    }
}
