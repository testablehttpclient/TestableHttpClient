namespace TestableHttpClient.Tests.HttpRequestMessageAsserterTests;

public sealed class Constructor
{
    [Fact]
    public void Constructor_NullRequestList_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new HttpRequestMessageAsserter(null!));
    }

    [Fact]
    public void Constructor_NullOptions_SetsDefault()
    {
        HttpRequestMessageAsserter sut = new([], null);
        Assert.NotNull(sut.Options);
    }

    [Fact]
    public void Constructor_NotNullOptions_SetsOptions()
    {
        TestableHttpMessageHandlerOptions options = new();
        HttpRequestMessageAsserter sut = new([], options);
        Assert.Same(options, sut.Options);
    }
}
