# TestableHttpClient

Using HttpClient in code that is unit tested is seen as rather difficult, this library aims to make it easier to assert the calls that are made via an HttpClient.

## How to install

This library is released as a NuGet package and can be installed via the NuGet manager in Visual Studio or by running the following command:

```
dotnet add package TestableHttpClient
```

## How to use

```c#
var testHandler = new TestableHttpMessageHandler();
var httpClient = new HttpClient(testHandler);

var result = await httpClient.GetAsync("http://httpbin.org/status/200");

testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/*");
```

More examples can be found in the [IntegrationTests project](test/TestableHttpClient.IntegrationTests)

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on how you can help us out.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [releases on this repository](https://github.com/dnperfors/TestableHttpClient/releases).

## Authors

* **David Perfors** - [dnperfors](https://github.com/dnperfors)

See also the list of [contributors](https://github.com/dnperfors/TestableHttpClient/contributors) who participated in this project.

## License

This project is released under the MIT license, see [LICENSE.md](LICENSE.md) for more information.

## Acknowledgments

This library is largely inspired by the HttpTest functionality from [Flurl](https://flurl.dev).  
A lot of the ideas came from the thread about unit testing HttpClient code in [this dotnet issue](https://github.com/dotnet/runtime/issues/14535).
