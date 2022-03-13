namespace TestableHttpClient.NFluent.Tests;

public partial class HttpResponseMessageChecksTests
{
    [Fact]
    public void HasReasonPhrase_WhenHttpResponseMessageIsNull_DoesFail()
    {
        HttpResponseMessage? sut = null;

        Check.ThatCode(() => Check.That(sut).HasReasonPhrase("OK"))
            .IsAFailingCheckWithMessage(
                "",
                "The checked response is null."
            );
    }

    [Fact]
    public void HasReasonPhrase_WhenReasonPhraseIsCorrect_DoesNotFail()
    {
        using var sut = new HttpResponseMessage(HttpStatusCode.OK);

        Check.That(sut).HasReasonPhrase("OK");
    }

    [Fact]
    public void HasReasonPhrase_WhenReasonPhraseIsNotCorrect_DoesFail()
    {
        using var sut = new HttpResponseMessage(HttpStatusCode.BadRequest);

        Check.ThatCode(() => Check.That(sut).HasReasonPhrase("OK"))
            .IsAFailingCheckWithMessage(
                "",
                "The checked response's reason phrase is not the expected reason phrase.",
                "The checked response's reason phrase:",
                "\t[\"Bad Request\"]",
                "The expected reason phrase:",
                "\t[\"OK\"]"
            );
    }

    [Fact]
    public void HasReasonPhrase_WhenReasonPhraseIsNotCorrectAndNotIsUsed_DoesNotFail()
    {
        using var sut = new HttpResponseMessage(HttpStatusCode.BadRequest);


        Check.That(sut).Not.HasReasonPhrase("OK");
    }

    [Fact]
    public void HasReasonPhrase_WhenReasonPhraseIsCorrectAndNotIsUsed_DoesFail()
    {
        using var sut = new HttpResponseMessage(HttpStatusCode.OK);

        Check.ThatCode(() => Check.That(sut).Not.HasReasonPhrase("OK"))
            .IsAFailingCheckWithMessage(
                "",
                "The checked response's reason phrase should not be the forbidden reason phrase.",
                "The checked response's reason phrase:",
                "\t[\"OK\"]",
                "The forbidden reason phrase:",
                "\t[\"OK\"]"
            );
    }
}
