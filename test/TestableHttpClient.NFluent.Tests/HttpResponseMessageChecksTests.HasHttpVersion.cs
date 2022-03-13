namespace TestableHttpClient.NFluent.Tests;

public partial class HttpResponseMessageChecksTests
{
    [Fact]
    public void HasHttpVersion_WhenHttpResponseMessageIsNull_DoesFail()
    {
        HttpResponseMessage? httpResponseMessage = null;

        Check.ThatCode(() => Check.That(httpResponseMessage).HasHttpVersion(HttpVersion.Version11))
            .IsAFailingCheckWithMessage(
                "",
                "The checked response is null."
            );

    }

    [Fact]
    public void HasHttpVersion_WhenVersionIsCorrect_DoesNotFail()
    {
        using var httpResponseMessage = new HttpResponseMessage()
        {
            Version = HttpVersion.Version11
        };

        Check.That(httpResponseMessage).HasHttpVersion(HttpVersion.Version11);
    }

    [Fact]
    public void HasHttpVersion_WhenVersionIsNotCorrect_DoesFail()
    {
        using var httpResponseMessage = new HttpResponseMessage
        {
            Version = HttpVersion.Version10
        };

        Check.ThatCode(() => Check.That(httpResponseMessage).HasHttpVersion(HttpVersion.Version11))
            .IsAFailingCheckWithMessage(
                "",
                "The checked response's version is not the expected version.",
                "The checked response's version:",
                "\t[1.0]",
                "The expected version:",
                "\t[1.1]"
            );
    }

    [Fact]
    public void HasHttpVersion_WhenVersionIsNotCorrectAndNotIsUsed_DoesNotFail()
    {
        using var httpResponseMessage = new HttpResponseMessage
        {
            Version = HttpVersion.Version10
        };

        Check.That(httpResponseMessage).Not.HasHttpVersion(HttpVersion.Version11);
    }

    [Fact]
    public void HasHttpVersion_WhenVersionIsCorrectAndNotIsUsed_DoesFail()
    {
        using var httpResponseMessage = new HttpResponseMessage
        {
            Version = HttpVersion.Version11
        };

        Check.ThatCode(() => Check.That(httpResponseMessage).Not.HasHttpVersion(HttpVersion.Version11))
            .IsAFailingCheckWithMessage(
                "",
                "The checked response's version should not be the forbidden version.",
                "The checked response's version:",
                "\t[1.1]",
                "The forbidden version:",
                "\t[1.1]"
            );
    }
}
