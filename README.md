# TestableHttpClient

![GitHub](https://img.shields.io/github/license/testablehttpclient/TestableHttpClient) ![GitHub Workflow Status](https://img.shields.io/github/workflow/status/testablehttpclient/TestableHttpClient/CI)

Creating unittest for code that uses `HttpClient` can be difficult to test. It requires a custom HttpMessageHandler or a mocked version. TestableHttpClient provides a testable version of HttpMessageHandler and several helper functions to configure the `TestableHttpHandler` and several ways to assert which requests were made.

## How to install

TestableHttpClient is released as a NuGet packages and can be installed via the NuGet manager in VisualStudio or by running the following command on the command line:
```
dotnet add package TestableHttpClient
```

## How to use TestableHttpClient

```csharp
var testHandler = new TestableHttpMessageHandler();
var httpClient = new HttpClient(testHandler); // or testHandler.CreateClient();

var result = await httpClient.GetAsync("http://httpbin.org/status/200");

testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/*");
```

More examples can be found in the [IntegrationTests project](test/TestableHttpClient.IntegrationTests)

## Supported .NET versions

TestableHttpClient is build as a netstandard2.0 library, so theoretically it can work on every .NET version that support netstandard2.0.
The following versions are being actively tested and thus supported:

- .NET Framework 4.6 and up
- .NET 6.0
- .NET 7.0

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
