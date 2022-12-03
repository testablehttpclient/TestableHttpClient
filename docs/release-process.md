# TestableHttpClient release process

This document describes how we release our libraries, how we version the libraries and how to describe changes for the libraries.

## NuGet packages

Every library that is release should generate a NuGet package via the `dotnet pack` command. This is currently done via the `dotnetcore` workflow on github. The packages are published as soon as a git tag is created.

All information for the package is described in either the `Directory.Build.props` files in the root directory, or in the `.csproj` file of the library itself.

The documentation for the specific library should be placed in a `README.md` file next to the `.csproj` file from that library. This way it is easier to update the documentation of the package on nuget.org.

## Changelog

The Changelog is kept in CHANGELOG.md and should be updated with every PR. On release a summary and the most important parts are mentioned.

For every release a milestone is created and all issues and PullRequests are linked to the milestone where they will appear in. This can than act as the official changelog.

## Versioning

All packages in this repository use the same version, mainly because the libraries are meant as framework specific extensions of the `TestableHttpClient` library. Therefore it would only lead to confusion when version numbers differ from each other.
