using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class MessageBuilderTests
{
    [Fact]
    public void BuildMessage_NoExpectedCountZeroActualCountNoConditions()
    {
        var result = MessageBuilder.BuildMessage(null, 0, []);

        Assert.Equal("Expected at least one request to be made, but no requests were made.", result);
    }

    [Theory]
    [InlineData(1, "one request was made")]
    [InlineData(2, "2 requests were made")]
    [InlineData(10, "10 requests were made")]
    public void BuildMessage_NoExpectedCountVariableActualCountNoConditions(int actualCount, string expectedMessage)
    {
        var result = MessageBuilder.BuildMessage(null, actualCount, []);

        Assert.Equal($"Expected at least one request to be made, and {expectedMessage}.", result);
    }

    [Fact]
    public void BuildMessage_ZeroExpectedCountZeroActualCountNoConditions()
    {
        var result = MessageBuilder.BuildMessage(0, 0, []);

        Assert.Equal($"Expected no requests to be made, and no requests were made.", result);
    }

    [Theory]
    [InlineData(1, "one request")]
    [InlineData(2, "2 requests")]
    [InlineData(10, "10 requests")]
    public void BuildMessage_VariableExpectedCountZeroActualCountNoConditions(int expectedCount, string expectedMessage)
    {
        var result = MessageBuilder.BuildMessage(expectedCount, 0, []);

        Assert.Equal($"Expected {expectedMessage} to be made, but no requests were made.", result);
    }

    [Theory]
    [InlineData("with condition 1", "condition 1")]
    [InlineData("with condition 1, condition 2", "condition 1", "condition 2")]
    [InlineData("with condition 1, condition 2, condition 3", "condition 1", "condition 2", "condition 3")]
    public void BuildMessage_NullExpectedCountZeroActualCountVariableAmountOfConditions(string expectedCondition, params string[] conditions)
    {
        var result = MessageBuilder.BuildMessage(null, 0, conditions);

        Assert.Equal($"Expected at least one request to be made {expectedCondition}, but no requests were made.", result);
    }

    [Theory]
    [InlineData("with condition 1", "condition 1")]
    [InlineData("with condition 1, condition 2", "condition 1", "condition 2")]
    [InlineData("with condition 1, condition 2, condition 3", "condition 1", "condition 2", "condition 3")]
    public void BuildMessage_NullExpectedCountOneActualCountVariableAmountOfConditions(string expectedCondition, params string[] conditions)
    {
        var result = MessageBuilder.BuildMessage(null, 1, conditions);

        Assert.Equal($"Expected at least one request to be made {expectedCondition}, and one request was made.", result);
    }
}
