﻿using System.Diagnostics;
using System.Threading;

using TestableHttpClient.Response;
using TestableHttpClient.Utils;

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
        var response = Assert.IsType<StatusCodeResponse>(sut);
        Assert.Equal(HttpStatusCode.Ambiguous, response.StatusCode);
    }

    [Fact]
    public void NoContent_ReturnsStatusCodeResponse()
    {
        var sut = NoContent();
        var response = Assert.IsType<StatusCodeResponse>(sut);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
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
        var sut = Delayed(NoContent(), 500);
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
        Assert.Throws<ArgumentNullException>(() => Configured(NoContent(), null!));
    }

    [Fact]
    public void Configured_ReturnsConfiguredResponse()
    {
        var sut = Configured(NoContent(), x => { });
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
        var sut = Sequenced(NoContent());
        Assert.IsType<SequencedResponse>(sut);
    }

    [Fact]
    public void Json_ReturnsJsonResponse()
    {
        var sut = Json("Charlie");
        Assert.IsType<JsonResponse>(sut);
    }
}
