﻿namespace TestableHttpClient.Response
{
    public abstract class ResponseBase : IResponse
    {
        public HttpStatusCode StatusCode
        {
            get;
#if NET6_0_OR_GREATER
            init;
#else
            set;
#endif
        } = HttpStatusCode.OK;
        
        protected virtual HttpResponseMessage GetResponse(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response = new();
            response.StatusCode = StatusCode;
            response.Content = GetContent(requestMessage);
            return response;
        }

        protected virtual HttpContent? GetContent(HttpRequestMessage requestMessage)
        {
            return null;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetResponse(requestMessage));
        }
    }
}
