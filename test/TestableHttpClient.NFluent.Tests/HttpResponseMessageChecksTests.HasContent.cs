namespace TestableHttpClient.NFluent.Tests;

public partial class HttpResponseMessageChecksTests
{
    [Fact]
    public void HasContent_WhenHttpResponseMessageIsNull_DoesFail()
    {
        HttpResponseMessage? sut = null;

        Check.ThatCode(() => Check.That(sut).HasContent())
            .IsAFailingCheckWithMessage(
                "",
                "The checked response is null."
            );
    }

    [Fact]
    public void HasContent_WhenContentIsNull_DoesFail()
    {
        using var sut = new HttpResponseMessage();

        Check.ThatCode(() => Check.That(sut).HasContent())
            .IsAFailingCheckWithMessage(
                "",
                "The checked response has no content, but content was expected."
            );
    }

    [Fact]
    public void HasContent_WhenContentIsNullAndNotEmpty_DoesFail()
    {
        using var sut = new HttpResponseMessage
        {
            Content = new StringContent("")
        };

        Check.ThatCode(() => Check.That(sut).HasContent())
            .IsAFailingCheckWithMessage(
                "",
                "The checked response has no content, but content was expected."
            );
    }

    [Fact]
    public void HasContent_WhenContentIsNotEmpty_DoesNotFail()
    {
        using var sut = new HttpResponseMessage
        {
            Content = new StringContent("Some Content")
        };

        Check.That(sut).HasContent();
    }

    [Fact]
    public void HasContent_WhenContentIsNullAndNotIsUsed_DoesNotFail()
    {
        using var sut = new HttpResponseMessage();

        Check.That(sut).Not.HasContent();
    }

    [Fact]
    public void HasContent_WhenContentIsEmptyAndNotIsUsed_DoesNotFail()
    {
        using var sut = new HttpResponseMessage
        {
            Content = new StringContent("")
        };

        Check.That(sut).Not.HasContent();
    }

    [Fact]
    public void HasContent_WhenContentIsNotNullAndNotIsUsed_DoesFail()
    {
        using var sut = new HttpResponseMessage
        {
            Content = new StringContent("Some content")
        };

        Check.ThatCode(() => Check.That(sut).Not.HasContent())
            .IsAFailingCheckWithMessage(
                "",
                "The checked response has content, but no content was expected."
            );
    }
}
