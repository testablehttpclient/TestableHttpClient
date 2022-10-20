# TestableHttpClient

![GitHub](https://img.shields.io/github/license/testablehttpclient/TestableHttpClient) ![GitHub Workflow Status](https://img.shields.io/github/workflow/status/testablehttpclient/TestableHttpClient/CI)

Using HttpClient in code that is unit tested is seen as rather difficult, these libraries aims to make it easier to assert the calls that are made via an HttpClient and to make assertions on the HttpResponseMessages.

This repository contains multiple related libraries. The separation is mainly made in order to separate dependencies and not to force a specific library to the user.

|Libary|Description|Main dependency|Nuget|
|------|-----------|---------------|-----|
|TestableHttpClient|Basic library for mocking HttpMessageHandler and to make manual assertions on HttpResponseMessage.|None|![Nuget](https://img.shields.io/nuget/v/TestableHttpClient)|
|TestableHttpClient.NFluent|Library containing [NFluent](https://github.com/tpierrain/NFluent) checks for HttpResponseMessage and TestableHttpMessageHandler.|NFluent|![Nuget](https://img.shields.io/nuget/v/TestableHttpClient.NFluent)|

## How to install

The libraries are released as a NuGet packages and can be installed via the NuGet manager in Visual Studio or by running one of the following commands:

```
dotnet add package TestableHttpClient
dotnet add package TestableHttpClient.NFluent
```

## How to use TestableHttpClient

```csharp
var testHandler = new TestableHttpMessageHandler();
var httpClient = new HttpClient(testHandler); // or testHandler.CreateClient();

var result = await httpClient.GetAsync("http://httpbin.org/status/200");

testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/*");
```

More examples can be found in the [IntegrationTests project](test/TestableHttpClient.IntegrationTests)

## How to use TestableHttpClient.NFluent to test responses

```csharp
var client = new HttpClient();

var result = await httpClient.GetAsync("https://httpbin.org/status/200");

Check.That(result).HasStatusCode(HttpStatusCode.OK).And.HasContentHeader("Content-Type", "*/json*");
```

## How to use TestableHttpClient.NFluent to which requests are made

```csharp
var testHandler = new TestableHttpMessageHandler();
var httpClient = new HttpClient(testHandler); // or testHandler.CreateClient();

var result = await httpClient.GetAsync("http://httpbin.org/status/200");

Check.That(testHandler).HasMadeRequestsTo("https://httpbin.org/*");
```

## Supported .NET versions

TestableHttpClient is build as a netstandard2.0 library, so theoretically it can work on every .NET version that support netstandard2.0.
However, only the following versions are being actively tested and thus supported:

- .NET Core 3.1
- .NET 6.0

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
