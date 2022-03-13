namespace TestableHttpClient.NFluent.Tests;

public partial class HttpResponseMessageChecksTests
{
    [Fact]
    public void HasResponseHeader_WhenHttpResponseMessageIsNull_DoesFail()
    {
        HttpResponseMessage? sut = null;

        Check.ThatCode(() => Check.That(sut).HasResponseHeader("Server"))
            .IsAFailingCheckWithMessage(
                "",
                "The checked response is null."
            );
    }

    [Fact]
    public void HasResponseHeader_WhenHeaderIsPresent_DoesNotFail()
    {
        using var sut = new HttpResponseMessage();
        sut.Headers.Add("Server", "nginx");

        Check.That(sut).HasResponseHeader("Server");
    }

    [Fact]
    public void HasResponseHeader_WhenHeaderIsNotPresent_DoesFail()
    {
        using var sut = new HttpResponseMessage();
        sut.Headers.ConnectionClose = true;

        Check.ThatCode(() => Check.That(sut).HasResponseHeader("Server"))
            .IsAFailingCheckWithMessage(
                "",
                "The checked response's headers does not contain the expected header.",
                "The checked response's headers:",
                "\t{\"Connection\"} (1 item)",
                "The expected header:",
                "\t[\"Server\"]"
            );
    }

    [Fact]
    public void HasResponseHeader_WhenHeaderIsNotPresentAndNotIsUsed_DoesNotFail()
    {
        using var sut = new HttpResponseMessage();


        Check.That(sut).Not.HasResponseHeader("Server");
    }

    [Fact]
    public void HasResponseHeader_WhenHeaderIsPresentAndNotIsUsed_DoesFail()
    {
        using var sut = new HttpResponseMessage();
        sut.Headers.Add("Server", "nginx");

        Check.ThatCode(() => Check.That(sut).Not.HasResponseHeader("Server"))
            .IsAFailingCheckWithMessage(
                "",
                "The checked response's headers should not contain the forbidden header.",
                "The checked response's headers:",
                "\t{\"Server\"} (1 item)",
                "The forbidden header:",
                "\t[\"Server\"]"
            );
    }
}
