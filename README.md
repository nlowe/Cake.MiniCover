# Cake.MiniCover

A [Cake](https://cakebuild.net) addin for [MiniCover](https://github.com/lucaslorentz/minicover)

> ***WIP***: None of this actually works at the moment due to how `MiniCover` is packaged.
>
> Track [lucaslorentz/minicover#31](https://github.com/lucaslorentz/minicover/issues/31) for details

## Usage

```csharp
#addin "Cake.MiniCover"

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

## License

`Cake.MiniCover` is licensed under the MIT License. It makes use of [MiniCover](https://github.com/lucaslorentz/minicover),
which is licensed under the MIT License. It is based off of
[`Cake.Common.Tools.OpenCover`](https://github.com/cake-build/cake/tree/develop/src/Cake.Common/Tools/OpenCover),
which is licensed under the MIT License.