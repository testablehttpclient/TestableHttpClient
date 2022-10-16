namespace TestableHttpClient.Response
{
    public class HttpResponse : IResponse
    {
        public HttpResponse() { }
        public HttpResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

        protected virtual Task<HttpContent?> GetContentAsync(HttpResponseContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<HttpContent?>(null);
        }

        public async Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.HttpResponseMessage.StatusCode = StatusCode;
            var content = await GetContentAsync(context, cancellationToken).ConfigureAwait(false);
            if (content is not null)
            {
                context.HttpResponseMessage.Content = content;
            }
        }
    }
}
