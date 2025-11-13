# Changelog
All notable changes to TestableHttpClient will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and 
this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.12] - unplanned
### Removed
- .NET 6.0 target, since it is no longer supported
- .NET Framework 4.6.2, 4.7.0 and 4.7.2, since these can't be tested using xUnit v3
### Added
- Support for .NET 9.0
- Support for .NET 10.0
### Changed
- The TestableHttpMessageHandler now makes a clone of the original request, so that the original request can be disposed.  
  This change also makes it possible to assert the content on .NET Framework.

## [0.11] - 2024-06-15
### Removed
- .NET 7.0 target, since it is no longer supported
- `ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler, string, bool)` and `ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler, string, bool, int)` have been removed. CaseInsensitivity is controlled by the `UriPatternMatchingOptions` that can be set on the `TestableHttpMessageHandler`.
- `WithRequestUri(this IHttpRequestMessagesCheck, string, bool)` and `WithRequestUri(this IHttpRequestMessagesCheck, string, bool, int)` have been removed. CaseInsensitivity is controlled by the `UriPatternMatchingOptions` that can be set on the `TestableHttpMessageHandler`.
- `WithQueryString` has been removed, since `ShouldHaveMadeRequestTo` and `WithRequestUri` now properly support querystrings.

### Changed
- Replaced Moq with NSubstitute in test project because of the SponsorLink dependencies.

### Added
- Support for .NET 8
- `CreateClient` now sets `https://localhost` as default BaseAddress on the created `HttpClient`.

## [0.10] - 2022-12-03
### Deprecated
- `ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler, string, bool)` and `ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler, string, bool, int)` have been deprecated. CaseInsensitivity is controlled by the `UriPatternMatchingOptions` that can be set on the `TestableHttpMessageHandler`.
- `WithRequestUri(this IHttpRequestMessagesCheck, string, bool)` and `WithRequestUri(this IHttpRequestMessagesCheck, string, bool, int)` have been deprecated. CaseInsensitivity is controlled by the `UriPatternMatchingOptions` that can be set on the `TestableHttpMessageHandler`.
- `WithQueryString` has been deprecated, since `ShouldHaveMadeRequestTo` and `WithRequestUri` now properly support querystrings.

### Removed
- `TestableHttpMessageHandler.SimulateTimeout` has been removed, and can be replaced with `RespondWith(Responses.Timeout())`.
- `TestableHttpMessageHandler.RespondWith(Func<HttpRequestMessage, HttpResponseMessage>)` has been removed, it's functionality is replaced by IResponse.
- `RespondWith(this TestableHttpMessageHandler, HttpResponseMessage)` has been removed, the response is modified with every call, so it doesn't work reliably and is different from how HttpClientHandler works, which creates a HttpResponseMessage for every request.
- `HttpResponseMessageBuilder` and `RespondWith(this TestableHttpMessageHandler, HttpResponseMessageBuilder)` has been removed, it's functionality can be replaced with ConfiguredResponse or a custom IResponse.
- `HttpResponseContext` now has an internal constructor instead of a public one.

### Added
- URI patterns now support query parameters and by default will use the unescaped values, note that the order is still important.
- URI pattern parsing is extended to be able to parse most URI's.
- `TestableHttpMessageHandler.ClearRequests` was added for situations where it is not possible to create and use a new instance.

### Changed
- Use the same parser for the assertion methods `WithRequestUri` (which is used by `ShouldHaveMadeRequestsTo`) as for the RoutingResponse functionality.  
  This introduced a breaking change where `*/customers` no longer validate the URI Path as a pattern that **ends** with `/customers`, but as an exact match for `/customers`.
- `RouteParserException` has been renamed to `UriPatternParserException`.
- Renamed `RoutingOptions` to `UriPatternMatchingOptions`.
- `SequencedResponse` now is able to recover from a reset.

## [0.9] - 2022-11-25
### Deprecated
- `Responses.NoContent()` has been deprecated, since it doesn't fit well with the rest of the API. Please use `Responses.StatusCode(HttpStatusCode.NoContent)` instead.

### Removed
- Official support for .NET Core 3.1 has been removed. This means we no longer provide a specific version for .NET Core 3.0 and we no longer test this version explicitly. Since we support .NET Standard 2.0, the library could still be used.
- TestableHttpClient.NFluent has been moved to it's own repository.
- `HttpRequestMessageExtensions` have been made internal.
- `HttpResponseMessageExtensions` have been removed, since it not needed for making HttpClients testable.

### Added
- Added `Responses.Route` that allows changing the response based on the url. The url supports patterns.

## [0.8] - 2022-11-08
### Deprecated
- `TestableHttpMessageHandler.SimulateTimeout` is deprecated, and can be replaced with `RespondWith(Responses.Timeout())`.
- `TestableHttpMessageHandler.RespondWith(Func<HttpRequestMessage, HttpResponseMessage>)` had been deprecated, it's functionality is replaced by IResponse.
- `RespondWith(this TestableHttpMessageHandler, HttpResponseMessage)` has been deprecated, the response is modified with every call, so it doesn't work reliably and is different from how HttpClientHandler works, which creates a HttpResponseMessage for every request.
- `HttpResponseMessageBuilder` and `RespondWith(this TestableHttpMessageHandler, HttpResponseMessageBuilder)` has been deprecated, it's functionality can be replaced with ConfiguredResponse or a custom IResponse.
- `HttpRequestMessageExtensions` and `HttpResponseMessageExtensions` were introduced as extensions for easier assertion. However, these types are intended for internal use and will be made internal next release.
- TestableHttpClient.NFluent is deprecated and will be removed in the next version.

### Added
- `CreateClient` now accepts `DelegateHandlers` in order to chain Handlers. The InnerHandler property of each handler is set automatically and the `TestableHttpMessageHandler` is automatically set as the last handler. This is showcased with Polly in the integration tests.
- Added support for .NET Framework 4.6.2, .NET Framework 4.7 and .NET Framework 4.8 by running the tests against these versions.
- Added support for .NET 7
- When validating requests, an `HttpRequestMessageAssertionException` will be thrown when the content of a request is disposed. This typically happens on .NET Framework when the runtime decides to use the older version of System.Net.Http.
- Added several `Responses`, including `Delayed`, `Timeout`, `Configured`, `Sequenced`, `StatusCode` and `Json`. These responses can now be used inside the `RespondWith`.
- Added the possibility to set and override the JsonSerializerOptions.

### Changed
- `TestableHttpClient` now works with the `Responses` class, making it easier to configure responses.
- When `HttpResponseMessage.Content` is null after `IResponse.ExecuteAsync` was called, an empty `StringContent` is added (Up until .NET 6.0, since Content is always filled there).
- The `HttpRequestMessage` is always added to the response, which is now possible, since we no longer allow reusing responses.
- Added `ConfigureAwait(false)` to all calls, since we now use async/await in the library.
- The check on request uri is now case insensitive by default, when test url's that are case sensitive set the `ignoreCase` parameter to false.
- The project has been moved to an organisation, so all the url's have been changed and an icon is added to the NuGet package.

## [0.7] - 2022-09-22
### Changed
- In 0.6 the debug symbols were embedded in the dll, so the pipeline couldn't upload the symbol package. This is corrected in 0.7 where the symbol package is correct.
- TestableHttpClient assembly is now CLSCompliant
- The mediaType parameter in `HttpResponseMessageBuilder.WithStringContent` is no longer nullable, like in .NET 7.0, it defaults to "text/plain" in other methods.
- `TestableHttpMessageHanlder.SimulateTimeout` functionality now actually cancels the request, so that it also works on .NET 7.0

### Added
- Support for .NET 6.0 has been added, although there were no code changes in the library, we now test if TestableHttpClient works with .NET 6.0. Besides that all assemblies are compiled using the .NET 6.0 SDK.
- The NuGet packages now support multiple target frameworks: .NET Standard 2.0, .NET Core 3.1 and .NET 6.0

### Removed
- Support for .NET Core 2.1 has been removed, although there were no code changes in the library, we no longer test if TestableHttpClient works with .NET Core 2.1.
- Support for .NET 5 has been removed, although there were no code changes in the library, we no longer test if TestableHttpClient works with .NET Core 5
- `ShouldNotHaveMadeRequests(this TestableHttpMessageHandler)` was removed in favour of the new `ShouldHaveMadeRequests(this HttpMessageHandler, 0)`.
- `ShouldNotHaveMadeRequestsTo(this TestableHttpMessageHandler, string)` was removed in favour of the new `ShouldHaveMadeRequestsTo(this HttpMessageHandler, string, 0)`.

## [0.6] - 2021-02-24
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
- Debug symbols are now embedded in the dll and the snupkg file is no longer uploaded seperately.
- TestableHttpClient is now being tested against multiple .net versions, currently these are: .NET Core 2.1, .NET Core 3.1 and .NET 5.0.
- `HasContent()` and `HasContent(string)` now return `false` when the actual content results in an empty string.
- Downgraded `System.Test.Json` and `NFluent` to the lowest supported version in the library. This is done based on the guidelines of [Microsoft](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/dependencies).
- Moved `RespondWith(HttpResponseMessage)` to an extension method, since it now uses `RespondWith(Func<HttpRequestMessage, HttpResponseMessage>)`.
- Moved `RespondWith(Action<HttpResponseMessageBuilder>)` to an extension method, since it now uses `RespondWith(Func<HttpRequestMessage, HttpResponseMessage>)`.
- Moved `SimulateTimeout()` to an extension method, since it now uses `RespondWith(Func<HttpRequestMessage, HttpResponseMessage>)`.
- The default response now sets the `HttpResponseMesage.RequestMessage`.
- `RespondWith(Action<HttpResponseMessageBuilder>)` sets `HttpResponseMessage.RequestMessage` by default before calling the builder action.

- Build pipeline now uses .NET SDK by default including NETAnalyzers and C# 9
- Build uses NerdBank.GitVersioning instead of GitVersion, since we have to specify the version number in the CHANGELOG.md anyways.

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
- It is now possible to use NFluent to check `TestableHttpMessageHandler` by using `Check.That(handler).HasMadeRequests()` and `Check.That(handler).HasMadeRequestsTo("https://github.com/testablehttpclient/testablehttpclient")`. All existing `With` checks are supported.
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

[0.12]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.11...v0.12
[0.11]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.10...v0.11
[0.10]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.9...v0.10
[0.9]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.8...v0.9
[0.8]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.7...v0.8
[0.7]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.6...v0.7
[0.6]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.5...v0.6
[0.5]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.4...v0.5
[0.4]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.3...v0.4
[0.3]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.2...v0.3
[0.2]: https://github.com/testablehttpclient/TestableHttpClient/compare/v0.1...v0.2
[0.1]: https://github.com/testablehttpclient/TestableHttpClient/releases/tag/v0.1
