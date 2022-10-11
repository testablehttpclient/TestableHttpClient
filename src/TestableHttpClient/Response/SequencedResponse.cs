﻿using System.Collections.Concurrent;

namespace TestableHttpClient.Response;

internal class SequencedResponse : IResponse
{
    private readonly Queue<IResponse> responses;
    public SequencedResponse(IEnumerable<IResponse> responses)
    {
        this.responses = new(responses);
    }
    public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage)
    {
        var response = GetResponse();
        return response.GetResponseAsync(requestMessage);
    }

    private IResponse GetResponse()
    {
        if (responses.Count == 1)
        {
            return responses.Peek();
        }
        else
        {
            return responses.Dequeue();
        }
    }
}
