# TestableHttpClient

![GitHub](https://img.shields.io/github/license/testablehttpclient/TestableHttpClient) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/testablehttpclient/TestableHttpClient/ci.yml) ![Nuget](https://img.shields.io/nuget/v/TestableHttpClient)  [![CII Best Practices](https://bestpractices.coreinfrastructure.org/projects/4010/badge)](https://bestpractices.coreinfrastructure.org/projects/4010)

Creating unittest for code that uses `HttpClient` can be difficult to test. It requires a custom HttpMessageHandler or a mocked version. TestableHttpClient provides a testable version of HttpMessageHandler and several helper functions to configure the `TestableHttpHandler` and several ways to assert which requests were made.

## How to install

TestableHttpClient is released as a NuGet packages and can be installed via the NuGet manager in VisualStudio or by running the following command on the command line:
```
dotnet add package TestableHttpClient
```

## How to use TestableHttpClient

The following code block shows the basic use case for asserting that certain requests are made:
```csharp
TestableHttpMessageHandler testHandler = new();
HttpClient httpClient = new(testHandler); // or testHandler.CreateClient();

var result = await httpClient.GetAsync("http://httpbin.org/status/200");

testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/*");
```

The default response is an empty response message with a 200 OK StatusCode, in order to return real content the response need to be configured:
```csharp
TestableHttpMessageHandler testHandler = new();
testHandler.RespondWith(Responses.Json(new { Hello: "World" }));
HttpClient httpClient = new(testHandler); // or testHandler.CreateClient();

var result = await httpClient.GetAsync("http://httpbin.org/status/200");

testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/*");
```

More examples can be found in the [IntegrationTests project](test/TestableHttpClient.IntegrationTests)

## URI Patterns

TestableHttpClient supports URI patterns in several places, mainly in:
- Response routing, where an URI pattern is used for matching the request URI to a response
- Assertions, where requests can be asserted against an URI pattern.

URI patterns are based on URI's as specified in [RFC 3986](https://www.rfc-editor.org/rfc/rfc3986), but allow the wildcard character `*` to specify optional parts of an URI.

An URI contains several components:
- The scheme of an URI is optional, but when given it should end with `://`. When not given `*://` is assumed.
- User Information (`username:password@`) is ignored and is not checked at all.
- The host is optional and when not given `*` is assumed. Both IP addresses and registered names are supported.
- The port is optional, but when ':' is provided after host, it should have a value.
- The path is optional, but should start with a `/`. When `/` is given, it can be followed by a `*` to match it with any path.
- Query parameters are optional, when given it should start with a `?`.
- Fragments are ignored, but should start with a `#`.

URI patterns differ from URI's in the following ways:
- Any character is allowed, for example: `myhost:myport` is a valid URI pattern, but not a valid URI. (and this will never match).
- No encoding is performed, and patterns are matched against the unescaped values of an URI.
- Patterns are not normalized, so a path pattern `/test/../example` will not work.

Some examples:

Uri pattern | Matches
------------|--------
\*|Matches any URL
\*://\*/\*?\* | Matches any URL
/get | Matches any URL that uses the path `/get`
http\*://\* | Matches any URL that uses the scheme `http` or `https` (or any other scheme that starts with `http`)
localhost:5000 | Matches any URL that uses localhost for the host and port 5000, no matter what scheme or path is used.

## Controlling the behaviour of pattern matching

TestableHttpClient has several options available that let you control different parts of the library. These options can be found on
the `TestableHttpMessageHandler.Options` and are passed to the `HttpRepsonseContext` and the `IHttpRequestMessagesCheck`.
The options include:
- `JsonSerializerOptions` for controlling the serialization of json content
- `UriPatternMatchingOptions` for controlling how the URI pattern matching works.

## Supported .NET versions

TestableHttpClient is build as a netstandard2.0 library, so theoretically it can work on every .NET version that support netstandard2.0.
The following versions are being actively tested and thus supported:

- .NET Framework 4.6, 4.7 and 4.8
- .NET 6.0
- .NET 8.0

These versions are supported as long as Microsoft supports them, we do our best to test and support newer versions as soon as possible.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on how you can help us out.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [releases on this repository](https://github.com/testablehttpclient/TestableHttpClient/releases).

Note that currently every library will always be released when a new tag is created, even though it might not have any changes.

## Authors

* **David Perfors** - [dnperfors](https://github.com/dnperfors)

See also the list of [contributors](https://github.com/testablehttpclient/TestableHttpClient/contributors) who participated in this project.

## License

This project is released under the MIT license, see [LICENSE.md](LICENSE.md) for more information.

## Acknowledgments

This project is largely inspired by the HttpTest functionality from [Flurl](https://flurl.dev).  
A lot of the ideas came from the thread about unit testing HttpClient code in [this dotnet issue](https://github.com/dotnet/runtime/issues/14535).
