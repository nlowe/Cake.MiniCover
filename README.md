# Cake.MiniCover

[![Build Status](https://travis-ci.org/nlowe/Cake.MiniCover.svg?branch=master)](https://travis-ci.org/nlowe/Cake.MiniCover) [![nuget](https://img.shields.io/nuget/v/Cake.MiniCover.svg)](https://www.nuget.org/packages/Cake.MiniCover/)

A [Cake](https://cakebuild.net) addin for [MiniCover](https://github.com/lucaslorentz/minicover)

## Usage

Until [lucaslorentz/minicover#31](https://github.com/lucaslorentz/minicover/issues/31) is
resolved, you need to call the `SetMiniCoverToolsProject` alias to locate the tools project:

```csharp
#addin "Cake.MiniCover"

SetMiniCoverToolsProject("./minicover/minicover.csproj");

// ...

Task("Coverage")
    .IsDependentOn("build")
    .Does(() => 
{
    MiniCover(tool =>
        {
            foreach(var project in GetFiles("./test/**/*.csproj"))
            {
                tool.DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings()
                {
                    // Required to keep instrumentation added by MiniCover
                    NoBuild = true,
                    Configuration = configuration
                });
            }
        },
        new MiniCoverSettings()
            .WithAssembliesMatching("./test/**/*.dll")
            .WithSourcesMatching("./src/**/*.cs")
            .GenerateReport(ReportType.CONSOLE | ReportType.XML)
    );
});

// ...

```

If you need more fine-graned control or have multiple test targets, you can call the aliases individually:

```csharp
#addin "Cake.MiniCover"

SetMiniCoverToolsProject("./minicover/minicover.csproj");

// ...

Task("Test::Prepare")
    .Does(() => 
{
    MiniCoverInstrument(
        new MiniCoverSettings()
            .WithAssembliesMatching("./test/**/*.dll")
            .WithSourcesMatching("./src/**/*.cs")
    );
    MiniCoverReset();
});

Task("Test")
    .IsDependentOn("Test::Prepare")
    .Does(() => 
{
    DotNetCoreTest(...);
    MiniCoverUninstrument();
    MiniCoverReport(new MiniCoverSettings().GenerateReport(ReportType.CONSOLE | ReportType.XML));
})
```

## License

`Cake.MiniCover` is licensed under the MIT License. It makes use of [MiniCover](https://github.com/lucaslorentz/minicover),
which is licensed under the MIT License. It is based off of
[`Cake.Common.Tools.OpenCover`](https://github.com/cake-build/cake/tree/develop/src/Cake.Common/Tools/OpenCover),
which is licensed under the MIT License.