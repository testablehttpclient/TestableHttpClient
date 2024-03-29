﻿using TestableHttpClient.Response;

using static TestableHttpClient.Responses;

namespace TestableHttpClient.Tests.Response;

public class ResponsesTests
{
    [Fact]
    public void Timeout_ReturnsTimeoutResponse()
    {
        var sut = Timeout();

        Assert.IsType<TimeoutResponse>(sut);
    }

    [Fact]
    public void StatusCode_ReturnsStatusCodeResponse()
    {
        var sut = StatusCode(HttpStatusCode.Ambiguous);
        var response = Assert.IsType<HttpResponse>(sut);
        Assert.Equal(HttpStatusCode.Ambiguous, response.StatusCode);
    }

    [Fact]
    public void Delayed_WithTimeSpan_WithNullResponse_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Delayed(null!, TimeSpan.FromSeconds(1)));
        Assert.Equal("delayedResponse", exception.ParamName);
    }

    [Fact]
    public void Delayed_WithTimeSpan_ReturnsDelayedResponse()
    {
        var sut = Delayed(StatusCode(HttpStatusCode.NoContent), TimeSpan.FromSeconds(1));
        Assert.IsType<DelayedResponse>(sut);
    }

    [Fact]
    public void Delayed_WithNullResponse_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Delayed(null!, 500));
        Assert.Equal("delayedResponse", exception.ParamName);
    }

    [Fact]
    public void Delayed_ReturnsDelayedResponse()
    {
        var sut = Delayed(StatusCode(HttpStatusCode.NoContent), 500);
        Assert.IsType<DelayedResponse>(sut);
    }

    [Fact]
    public void Configured_NullResponse_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Configured(null!, _ => { }));
    }

    [Fact]
    public void Configured_NullConfigurationAction_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Configured(StatusCode(HttpStatusCode.NoContent), null!));
    }

    [Fact]
    public void Configured_ReturnsConfiguredResponse()
    {
        var sut = Configured(StatusCode(HttpStatusCode.NoContent), x => { });
        Assert.IsType<ConfiguredResponse>(sut);
    }

    [Fact]
    public void Sequenced_NoContent_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Sequenced());
    }

    [Fact]
    public void Sequenced_NullContent_ThrowsArgumentNullException()
    {
        IResponse[] responses = null!;
        Assert.Throws<ArgumentNullException>(() => Sequenced(responses));
    }

    [Fact]
    public void Sequenced_ReturnsSequencedResponse()
    {
        var sut = Sequenced(StatusCode(HttpStatusCode.NoContent));
        Assert.IsType<SequencedResponse>(sut);
    }

    [Fact]
    public void Text_ReturnsTextResponse()
    {
        var sut = Text("Hello world");
        TextResponse response = Assert.IsType<TextResponse>(sut);
        Assert.Equal("Hello world", response.Content);
        Assert.Equal(Encoding.UTF8, response.Encoding);
        Assert.Equal("text/plain", response.MediaType);
    }

    [Fact]
    public void Text_WithCustomMediaType_ReturnsTextResponse()
    {
        var sut = Text("Hello world", mediaType: "text/xml");
        TextResponse response = Assert.IsType<TextResponse>(sut);
        Assert.Equal("Hello world", response.Content);
        Assert.Equal(Encoding.UTF8, response.Encoding);
        Assert.Equal("text/xml", response.MediaType);
    }

    [Fact]
    public void Json_ReturnsJsonResponse()
    {
        var sut = Json("Charlie");
        JsonResponse response = Assert.IsType<JsonResponse>(sut);
        Assert.Equal("Charlie", response.Content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.ContentType);
    }

    [Fact]
    public void Json_WithCustomContentType_ReturnsJsonResponse()
    {
        var sut = Json("Charlie", "application/problem+json");
        JsonResponse response = Assert.IsType<JsonResponse>(sut);
        Assert.Equal("Charlie", response.Content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/problem+json", response.ContentType);
    }

    [Fact]
    public void Json_WithStatusCode_ReturnsJsonResponse()
    {
        var sut = Json("Charlie", HttpStatusCode.NotFound);
        JsonResponse response = Assert.IsType<JsonResponse>(sut);
        Assert.Equal("Charlie", response.Content);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/json", response.ContentType);
    }

    [Fact]
    public void Json_WithStatusCodeAndContentType_ReturnsJsonResponse()
    {
        var sut = Json("Charlie", HttpStatusCode.NotFound, "application/problem+json");
        JsonResponse response = Assert.IsType<JsonResponse>(sut);
        Assert.Equal("Charlie", response.Content);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.ContentType);
    }

    [Fact]
    public void Extensions_WithCustomExtension_ReturnsCorrectResponse()
    {
        var sut = Extensions.ServiceUnavailable();
        HttpResponse response = Assert.IsType<HttpResponse>(sut);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }

    [Fact]
    public void Route_WithNullBuilder_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Route(null!));
    }

    [Fact]
    public void Route_WithNonNullBuilder_CallsBuilderAndReturnsRoutingResponse()
    {
        bool builderWasCalled = false;
        void builder(IRoutingResponseBuilder _) => builderWasCalled = true;
        IResponse result = Route(builder);

        Assert.True(builderWasCalled);
        Assert.IsType<RoutingResponse>(result);
    }
}

internal static class TestResponseExtensions
{
    public static IResponse ServiceUnavailable(this IResponsesExtensions _) => new HttpResponse(HttpStatusCode.ServiceUnavailable);
}
