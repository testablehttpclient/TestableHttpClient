using System;
using System.Net.Http;

using Moq;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public static class Its
    {
        public static Func<HttpRequestMessage, bool> AnyPredicate()
        {
            return It.IsAny<Func<HttpRequestMessage, bool>>();
        }
    }
}
