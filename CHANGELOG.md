# Changelog
All notable changes to TestableHttpClient will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and 
this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [unreleased]
### Deprecated
- `ShouldNotHaveMadeRequests(this TestableHttpMessageHandler)` will be removed in favour of the new `ShouldHaveMadeRequests(this HttpMessageHandler, 0)`.
- `ShouldNotHaveMadeRequestsTo(this TestableHttpMessageHandler, string)` will be removed in favour of the new `ShouldHaveMadeRequestsTo(this HttpMessageHandler, string, 0)`.

### Added
- `WithQueryString(this IHttpRequestMessageCheck, string)` to test the querystring without url encoding.
- `RespondWith(Func<HttpRequestMessage, HttpResponseMessage)` to configure a factory method that is called when making a request.
- `ShouldHaveMadeRequests(this TestableHttpMessageHandler, int)` to test that a certain amount of requests are made.
- `ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler, string, int)` to test that a certain amount of requests are made.
- `public static IHttpRequestMessagesCheck HasMadeRequests(this ICheck<TestableHttpMessageHandler?>, int)` to test that a certain amount of requests are made.
- `public static IHttpRequestMessagesCheck HasMadeRequestsTo(this ICheck<TestableHttpMessageHandler?>, string, int)` to test that a certain amount of requests are made.

### Changed
- TestableHttpClient is now being tested against multiple .net versions, currently these are: .NET Core 2.1, .NET Core 3.1 and .NET 5.0.
- `HasContent()` and `HasContent(string)` now return `false` when the actual content results in an empty string.
- Downgraded `System.Test.Json` and `NFluent` to the lowest supported version in the library. This is done based on the guidelines of [Microsoft](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/dependencies).
- Moved `RespondWith(HttpResponseMessage)` to an extension method, since it now uses `RespondWith(Func<HttpRequestMessage, HttpResponseMessage>)`.
- Moved `RespondWith(Action<HttpResponseMessageBuilder>)` to an extension method, since it now uses `RespondWith(Func<HttpRequestMessage, HttpResponseMessage>)`.
- Moved `SimulateTimeout()` to an extension method, since it now uses `RespondWith(Func<HttpRequestMessage, HttpResponseMessage>)`.
- The default response now sets the `HttpResponseMesage.RequestMessage`.
- `RespondWith(Action<HttpResponseMessageBuilder>)` sets `HttpResponseMessage.RequestMessage` by default before calling the builder action.

- Build pipeline now uses .NET SDK by default including NETAnalyzers and C# 9

### Removed
- `WithUriPattern(this IHttpRequestMessagesCheck, string)` was removed in favour of `WithRequestUri(this IHttpRequestMessagesCheck, string)`
- `With(Func<HttpRequestMessage, bool>, string)` was removed in favour of `WithFilter(Func<HttpRequestMessage, bool>, string)`, since it conflicts with the language keyword `with`.
- `Times(int)` was removed in favour of the `With*` methods with an `int` parameter.

## [0.5] - 2020-06-25
### Deprecated
- `WithUriPattern(this IHttpRequestMessagesCheck, string)` will be removed in favour of `WithRequestUri(this IHttpRequestMessagesCheck, string)`
- `With(Func<HttpRequestMessage, bool>, string)` will be removed in favour of `WithFilter(Func<HttpRequestMessage, bool>, string)`, since it conflicts with the language keyword `with`.
- `Times(int)` will be removed in favour of the `With*` methods with an `int` parameter.

### Added
- It is now possible to use NFluent to check `TestableHttpMessageHandler` by using `Check.That(handler).HasMadeRequests()` and `Check.That(handler).HasMadeRequestsTo("https://github.com/dnperfors/testablehttpclient")`. All existing `With` checks are supported.
- All `With*` methods got an extra overload to specify the exact number of expected requests. This is instead of the `Times` method.

### Changed
- Introduced `IHttpRequestMessagesCheck` as the public interface for all checks on requests made to `TestableHttpMessageHandler`. It contains the following api:
  - `WithFilter(Func<HttpRequestMessage, bool>, string)`
  - `WithFilter(Func<HttpRequestMessage, bool>, int, string)`
  - `WithFilter(Func<HttpRequestMessage, bool>, int?, string)`
- Moved some api's from `TestableHttpMessageHandler` to `TestableHttpMessageHandlerAssertionExtensions`:
  - `ShouldHaveMadeRequests(this TestableHttpMessageHandler)`
  - `ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler, string)`
  - `ShouldNotHaveMadeRequests(this TestableHttpMessageHandler)`
  - `ShouldNotHaveMadeRequestsTo(this TestableHttpMessageHandler, string)`
- Moved most methods from `HttpRequestMessageAsserter` to `HttpRequestMessagesCheckExtensions`:
  - `WithRequestUri(this IHttpRequestMessagesCheck, string)` which is renamed from `WithUriPattern(this IHttpRequestMessagesCheck, string)`
  - `WithHttpMethod(this IHttpRequestMessagesCheck, HttpMethod)`
  - `WithHttpVersion(this IHttpRequestMessagesCheck, Version)`
  - `WithRequestHeader(this IHttpRequestMessagesCheck, string)`
  - `WithRequestHeader(this IHttpRequestMessagesCheck, string, string)`
  - `WithContentHeader(this IHttpRequestMessagesCheck, string)`
  - `WithContentHeader(this IHttpRequestMessagesCheck, string, string)`
  - `WithHeader(this IHttpRequestMessagesCheck, string)`
  - `WithHeader(this IHttpRequestMessagesCheck, string, string)`
  - `WithContent(this IHttpRequestMessagesCheck, string)`
  - `WithJsonContent(this IHttpRequestMessagesCheck, object)`
  - `WithFormUrlEncodedContent(this IHttpRequestMessagesCheck, IEnumerable<KeyValuePair<string, string>)`

### Removed
- `HttpRequestMessageAsserter` is made internal.

## [0.4] - 2020-05-26
### Removed 
- the following deprecated api's are removed:
  - `HttpResponseMessageBuilder.WithVersion(Version)`
  - `HttpResponseMessageBuilder.WithStatusCode(HttpStatusCode)`
  - `HttpResponseMessageBuilder.WithHeaders(Actions<HttpResponseHeaders>)`
  - `HttpResponseMessageBuilder.WithHeader(string, string)`
  - `HttpResponseMessageExtensions.HasHttpVersion(HttpResponseMessage, string)`

### Added
- `TestableHttpMessageHandlerExtensions.CreateClient(TestableHttpMessageHandler)`
- `TestableHttpMessageHandlerExtensions.CreateClient(TestableHttpMessageHandler, Action<HttpClient>)`

### Changed
- Example on how to configure IHttpClientFactory now uses dependency injection, just how you would use it in real life.

## [0.3] - 2020-05-24
### Added
- Examples on how to use TestableHttpClient in combination with IHttpClientFactory. Note that this is revised in the next release.
- `HttpRequestMessageExtensions.HasContent(HttpRequestMessage)`
- `HttpResponseMessageExtensions.HasContent(HttpResponseMessage)`

### Changed
- Improve error messages in NFluent checks
- Improve release pipeline
- Renamed several api's:
  - `HttpResponseMessageBuilder.WithVersion(Version)` is renamed to `WithHttpVersion(Version)`
  - `HttpResponseMessageBuilder.WithStatusCode(HttpStatusCode)` is renamed to `WithHttpStatusCode(HttpStatusCode)`
  - `HttpResponseMessageBuilder.WithHeaders(Actions<HttpResponseHeaders>)` is renamed to `WithResponseHeaders(Actions<HttpResponseHeaders>)`
  - `HttpResponseMessageBuilder.WithHeader(string, string)` is renamed to `WithResponseHeader(string, string)`

### Deprecated
- the following api's are deprecated because they are renamed:
  - `HttpResponseMessageBuilder.WithVersion(Version)`
  - `HttpResponseMessageBuilder.WithStatusCode(HttpStatusCode)`
  - `HttpResponseMessageBuilder.WithHeaders(Actions<HttpResponseHeaders>)`
  - `HttpResponseMessageBuilder.WithHeader(string, string)`
- the following api is deprecated because it is inconsistent:
  - `HttpResponseMessageExtensions.HasHttpVersion(HttpResponseMessage, string)`

## [0.2] - 2020-05-10
### Added
- TestableHttpClient.NFluent project. This project provides NFluent checks to check HttpResponseMessages. The following checks can be used:
  - `HasHttpStatusCode(ICheck<HttpResponseMessage>, HttpStatusCode)`
  - `HasReasonPhrase(ICheck<HttpResponseMessage>, string)`
  - `HasHttpVersion(ICheck<HttpResponseMessage>, Version)`
  - `HasResponseHeader(ICheck<HttpResponseMessage>, string)`
  - `HasResponseHeader(ICheck<HttpResponseMessage>, string, string)`
  - `HasContentHeader(ICheck<HttpResponseMessage>, string)`
  - `HasContentHeader(ICheck<HttpResponseMessage>, string, string)`
  - `HasContent(ICheck<HttpResponseMessage>)`
  - `HasContent(ICheck<HttpResponseMessage>, string)`
- `HttpResponseMessageExtensions` to easily check information on `HttpResponseMessages`
  - `HasHttpVersion(HttpResponseMessage, Version)`
  - `HasHttpVersion(HttpResponseMessage, string)`
  - `HasHttpStatusCode(HttpResponseMessage, HttpStatusCode)`
  - `HasReasonPhrase(HttpResponseMessage, string)`
  - `HasResponseHeader(HttpResponseMessage, string)`
  - `HasResponseHeader(HttpResponseMessage, string, string)`
  - `HasContentHeader(HttpResponseMessage, string)`
  - `HasContentHeader(HttpResponseMessage, string, string)`
  - `HasContent(HttpResponseMessage, string)`

## [0.1] - 2020-03-26
### Added
- `TestableHttMessageHandler` to record all requests that are made with an `HttpClient`
  - `RespondWith(HttpResponseMessage)`
  - `RespondWith(Action<HttpResponseMessageBuilder>)`
  - `SimulateTimeout()`
  - `ShouldHaveMadeRequests()`
  - `ShouldHaveMadeRequestsTo(string)`
  - `ShouldNotHaveMadeRequests()`
  - `ShouldNotHaveMadeRequestsTo(string)`
- `HttpResponseMessageBuilder` for creating HttpResponseMessages in a fluent way
  - `WithVersion(Version)`
  - `WithStatusCode(HttpStatusCode)`
  - `WithHeaders(Action<HttpResponseHeaders>)`
  - `WithHeader(string, string)`
  - `WithContent(HttpContent)`
  - `WithStringContent(string)`
  - `WithStringContent(string, Encoding)`
  - `WithStringContent(string, Encoding, string)`
  - `WithJsonContent(object)`
  - `WithJsonContent(object, string)`
  - `WithRequestMessage(HttpRequestMessage)`
  - `Build()`
- `HttpRequestMessageAsserter` to validate that certain requests are made. This can be requested by calling `TestableHttpMessageHandler.ShouldHaveMadeRequests()` or `TestableHttpMessageHandler.ShouldHaveMadeRequestsTo(string)`. The following methods are available:
  - `With(Func<HttpRequestMessage, bool>, string)`
  - `WithUriPattern(string)`
  - `WithHttpMethod(HttpMethod)`
  - `WithHttpVersion(Version)`
  - `WithRequestHeader(string)`
  - `WithRequestHeader(string, string)`
  - `WithContentHeader(string)`
  - `WithContentHeader(string, string)`
  - `WithHeader(string)`
  - `WithHeader(string, string)`
  - `WithContent(string)`
  - `WithJsonContent(object)`
  - `WithFormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>>)`
  - `Times(int)`
- `HttpRequestMessageExtensions` with extension methods for testing a data on HttpRequestMessages
  - `HasHttpVersion(HttpRequestMessage, Version)`
  - `HasHttpVersion(HttpRequestMessage, string)`
  - `HasHttpMethod(HttpRequestMessage, HttpMethod)`
  - `HasHttpMethod(HttpRequestMessage, string)`
  - `HasRequestMessage(HttpRequestMessage, string)`
  - `HasRequestMessage(HttpRequestMessage, string, string)`
  - `HasContentMessage(HttpRequestMessage, string)`
  - `HasContentMessage(HttpRequestMessage, string, string)`
  - `HasMatchingUri(HttpRequestMessage, string)`
  - `HasContent(HttpRequestMessage, string)`
- Created an integrationtest project that showcases all the basic features
- Created readme file
- Automatically build project when pushing changes to github and when creating a pull request
- Automatically deploy to NuGet when creating a tag in github

[0.5]: https://github.com/dnperfors/TestableHttpClient/compare/v0.4...v0.5
[0.4]: https://github.com/dnperfors/TestableHttpClient/compare/v0.3...v0.4
[0.3]: https://github.com/dnperfors/TestableHttpClient/compare/v0.2...v0.3
[0.2]: https://github.com/dnperfors/TestableHttpClient/compare/v0.1...v0.2
[0.1]: https://github.com/dnperfors/TestableHttpClient/releases/tag/v0.1
