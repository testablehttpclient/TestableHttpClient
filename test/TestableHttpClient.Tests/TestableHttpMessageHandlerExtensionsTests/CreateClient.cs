using System.Reflection;

namespace TestableHttpClient.Tests;

public partial class TestableHttpMessageHandlerExtensionsTests
{
    [Fact]
    public void CreateClient_NullTestableHttpMessageHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient());
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void CreateClient_CorrectTestableHttpMessageHandler_AddsHandlerToHttpClient()
    {
        using TestableHttpMessageHandler sut = new();

        using var client = sut.CreateClient();

        var handler = GetPrivateHandler(client);

        Assert.Same(sut, handler);
    }

    [Fact]
    public void CreateClient_NullDelegateHandler_ThrowsArgumentNullException()
    {
        using TestableHttpMessageHandler sut = new();
        DelegatingHandler handler = null!;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(handler));
        Assert.Equal("httpMessageHandlers", exception.ParamName);
    }

    private static HttpMessageHandler? GetPrivateHandler(HttpClient client)
    {
        var privateFields = client.GetType().BaseType?.GetFields(BindingFlags.Instance | BindingFlags.NonPublic) ?? Enumerable.Empty<FieldInfo>();
        var handlerField = privateFields.FirstOrDefault(x => x.FieldType == typeof(HttpMessageHandler));
        if (handlerField == null)
        {
            Assert.True(false, "Can't find the private HttpMessageHandler field on HttpClient.");
            return null;
        }
        return handlerField.GetValue(client) as HttpMessageHandler;
    }
}
