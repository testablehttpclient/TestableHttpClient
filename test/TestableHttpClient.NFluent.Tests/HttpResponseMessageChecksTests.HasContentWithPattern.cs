using System.Net.Http;

using NFluent;
using NFluent.Helpers;

using Xunit;

namespace TestableHttpClient.NFluent.Tests
{
    public partial class HttpResponseMessageChecksTests
    {
        [Fact]
        public void HasContentWithPattern_WhenHttpResponseMessageIsNull_DoesFail()
        {
            HttpResponseMessage? sut = null;

            Check.ThatCode(() => Check.That(sut).HasContent(""))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response is null."
                );
        }

        [Fact]
        public void HasContentWithPattern_WhenContentIsNull_DoesFail()
        {
            using var sut = new HttpResponseMessage();

            Check.ThatCode(() => Check.That(sut).HasContent(""))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content should be the expected content.",
                    "The expected content:",
                    "\t[\"\"]"
                );
        }

        [Fact]
        public void HasContentWithPattern_WhenContentIsNullAndExpectedContentIsNull_DoesNotFail()
        {
            using var sut = new HttpResponseMessage();

            Check.That(sut).HasContent(null);
        }

        [Fact]
        public void HasContentWithPattern_WhenContentIsNullAndExpectedContentIsNullAndNotIsUsed_DoesFail()
        {
            using var sut = new HttpResponseMessage();

            Check.ThatCode(() => Check.That(sut).Not.HasContent(null))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content should not be null."
                );
        }

        [Fact]
        public void HasContentWithPattern_WhenContentIsNotNullAndExpectedContentIsNull_DoesFail()
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("")
            };

            Check.ThatCode(() => Check.That(sut).HasContent(null))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content should be null."
                );
        }

        [Fact]
        public void HasContentWithPattern_WhenContentIsNotNullAndExpectedContentIsNullAndNotIsUsed_DoesNotFail()
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("")
            };

            Check.That(sut).Not.HasContent(null);
        }

        [Fact]
        public void HasContentWithPattern_WhenContentIsNullAndNotIsUsed_DoesNotFail()
        {
            using var sut = new HttpResponseMessage();

            Check.That(sut).Not.HasContent("");
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello World")]
        public void HasContentWithPattern_WhenContentIsExactMatch_DoesNotFail(string content)
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent(content)
            };

            Check.That(sut).HasContent(content);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello World")]
        public void HasContentWithPattern_WhenContentIsNoMatch_DoesFail(string expectedContent)
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("Greetings Martian")
            };

            Check.ThatCode(() => Check.That(sut).HasContent(expectedContent))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content should be the expected content.",
                    "The expected content:",
                    $"\t[\"{expectedContent}\"]"
                );
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello World")]
        public void HasContentWithPattern_WhenContentIsExactMatchAndNotIsUsed_DoesFail(string content)
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent(content)
            };

            Check.ThatCode(() => Check.That(sut).Not.HasContent(content))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content should not be the forbidden content.",
                    "The forbidden content:",
                    $"\t[\"{content}\"]"
                );
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello World")]
        public void HasContentWithPattern_WhenContentIsNoMatchAndNotIsUsed_DoesNotFail(string expectedContent)
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("Greetings Martian")
            };

            Check.That(sut).Not.HasContent(expectedContent);
        }

        [Theory]
        [InlineData("*")]
        [InlineData("Hello*")]
        [InlineData("*World")]
        public void HasContentWithPattern_WhenContentIsPatternMatch_DoesNotFail(string pattern)
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("Hello World")
            };

            Check.That(sut).HasContent(pattern);
        }

        [Theory]
        [InlineData("Hello*")]
        [InlineData("*World")]
        public void HasContentWithPattern_WhenContentIsNotPatternMatch_DoesFail(string pattern)
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("Greetings Martian")
            };

            Check.ThatCode(() => Check.That(sut).HasContent(pattern))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content does not match the expected pattern.",
                    "The expected content pattern:",
                    $"\t[\"{pattern}\"]"
                );
        }

        [Theory]
        [InlineData("*")]
        [InlineData("Hello*")]
        [InlineData("*World")]
        public void HasContentWithPattern_WhenContentIsPatternMatchAndNotIsUsed_DoesFail(string pattern)
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("Hello World")
            };

            Check.ThatCode(() => Check.That(sut).Not.HasContent(pattern))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content should not match the forbidden pattern.",
                    "The forbidden content pattern:",
                    $"\t[\"{pattern}\"]"
                );
        }

        [Theory]
        [InlineData("Hello*")]
        [InlineData("*World")]
        public void HasContentWithPattern_WhenContentIsNotPatternMatchAndNotIsUsed_DoesNotFail(string pattern)
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("Greetings Martian")
            };

            Check.That(sut).Not.HasContent(pattern);
        }
    }
}
