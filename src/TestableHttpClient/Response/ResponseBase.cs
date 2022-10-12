namespace TestableHttpClient.Response
{
    public abstract class ResponseBase : IResponse
    {
        protected virtual HttpResponseMessage GetResponse(HttpRequestMessage requestMessage)
        {
            return new HttpResponseMessage();
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetResponse(requestMessage));
        }
    }
}
