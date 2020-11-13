using System;
using System.Net.Http;

using NFluent;
using NFluent.Helpers;

using Xunit;

namespace TestableHttpClient.NFluent.Tests
{
    public partial class HttpResponseMessageChecksTests
    {
        [Fact]
        public void HasContentWithEmptyPattern_WhenHttpResponseMessageIsNull_DoesFail()
        {
            HttpResponseMessage? sut = null;

            Check.ThatCode(() => Check.That(sut).HasContent(""))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response is null."
                );
        }

        [Fact]
        public void HasContentWithEmptyPattern_WhenContentIsNull_DoesNotFail()
        {
            using var sut = new HttpResponseMessage();

            Check.That(sut).HasContent("");
        }

        [Fact]
        public void HasContentWithEmptyPattern_WhenContentIsEmpty_DoesNotFail()
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("")
            };

            Check.That(sut).HasContent("");
        }

        [Fact]
        public void HasContentWithEmptyPattern_WhenContentIsNull_DoesFail()
        {
            using var sut = new HttpResponseMessage();

            Check.ThatCode(() => Check.That(sut).HasContent("Some content"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content should be the expected content.",
                    "The checked response's content:",
                    "\t[\"\"]",
                    "The expected content:",
                    "\t[\"Some content\"]"
                );
        }

#nullable disable
        [Fact]
        public void HasContentWithPattern_WhenExpectedContentIsNull_DoesFail()
        {
            using var sut = new HttpResponseMessage();

            Check.ThatCode(() => Check.That(sut).HasContent(null))
                .IsAFailingCheckWithMessage(
                "",
                "The expected content should not be null, but it is."
                );
        }

        [Fact]
        public void HasContentWithPattern_WhenExpectedContentIsNullAndNotIsUsed_DoesFail()
        {
            using var sut = new HttpResponseMessage();

            Check.ThatCode(() => Check.That(sut).Not.HasContent(null))
                .Throws<InvalidOperationException>();
        }
#nullable restore

        [Fact]
        public void HasContentWithPattern_WhenContentIsNullAndNotIsUsed_DoesNotFail()
        {
            using var sut = new HttpResponseMessage();

            Check.ThatCode(() => Check.That(sut).Not.HasContent(""))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content should not be the forbidden content.",
                    "The checked response's content:",
                    $"\t[\"\"]",
                    "The forbidden content:",
                    $"\t[\"\"]"
                );
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
                    "The checked response's content:",
                    "\t[\"Greetings Martian\"]",
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
                    "The checked response's content:",
                    $"\t[\"{content}\"]",
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
                    "The checked response's content:",
                    "\t[\"Greetings Martian\"]",
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
                    "The checked response's content:",
                    "\t[\"Hello World\"]",
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
