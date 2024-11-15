using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

internal static class Args
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Non-substitutable member", "NS1004:Argument matcher used with a non-virtual member of a class.", Justification = "This is a custom matcher.")]
    public static ref Func<HttpRequestMessage, bool> AnyPredicate() => ref Arg.Any<Func<HttpRequestMessage, bool>>();
}
