﻿using TestableHttpClient.Response;

namespace TestableHttpClient;
/// <summary>
/// A testable HTTP message handler that captures all requests and always returns the same response.
/// </summary>
public class TestableHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpRequestMessage> httpRequestMessages = new();

    private IResponse response = new HttpResponse(HttpStatusCode.OK);

    public TestableHttpMessageHandlerOptions Options { get; } = new TestableHttpMessageHandlerOptions();

    /// <summary>
    /// Gets the collection of captured requests made using this HttpMessageHandler.
    /// </summary>
    public IEnumerable<HttpRequestMessage> Requests => httpRequestMessages;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        httpRequestMessages.Enqueue(request);

        HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(request, httpRequestMessages, responseMessage, Options);
        await response.ExecuteAsync(context, cancellationToken).ConfigureAwait(false);

        responseMessage.RequestMessage ??= request;

#if !NET6_0_OR_GREATER
        responseMessage.Content ??= new StringContent("");
#endif

        return responseMessage;
    }

    /// <summary>
    /// Configure a response that creates a <see cref="HttpResponseMessage"/> that should be returned for a request.
    /// </summary>
    /// <param name="response">The response that should be created.</param>
    /// <remarks>By default, each request will receive a new response, however this is dependent on the implementation.</remarks>
    /// <example>
    /// testableHttpMessageHandler.RespondWith(Responses.StatusCode(HttpStatusCode.OK));
    /// </example>
    public void RespondWith(IResponse response)
    {
        this.response = response ?? throw new ArgumentNullException(nameof(response));
    }

    /// <summary>
    /// Clear the registration of requests that were made with this handler.
    /// </summary>
    /// Sometimes the TestableHttpMessageHandler can't be replaced with a new instance, but it can be cleared.
    /// The configuration is not cleared and will be kept the same.
    /// <remarks>The configuration itself (Options and the configured IResponse) will not be cleared or reset.</remarks>
    public void ClearRequests()
    {
        httpRequestMessages.Clear();
    }
}
