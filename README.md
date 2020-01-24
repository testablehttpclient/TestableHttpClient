# httpclienttesthelpers
Using HttpClient in code that is unittested is seen as rather difficult, this library aims to make it easier to assert the calls that are made via an HttpClient.

This library is largly based on the HttpTest functionality from [Flurl](https://flurl.dev), but is focused around the use of HttpClient directly. Since I intend to replace my usage of Flurl.Http with this library, I kept the interface very similar.

# Todo before release
The following assertions should be able in the first release:
- WithRequestBody: Compare the body as text
- WithContentType: Or headers in the body of a httprequestmessage

Preferably the follwing setup for responses:
- easier response body creation?

