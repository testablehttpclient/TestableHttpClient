using System;
using System.Net.Http;

namespace TestableHttpClient
{
    public interface IHttpRequestMessagesCheck
    {
        IHttpRequestMessagesCheck Times(int count);
        IHttpRequestMessagesCheck With(Func<HttpRequestMessage, bool> requestFilter, string condition);
    }
}
