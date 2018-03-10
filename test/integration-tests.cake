#r "./_addin/Cake.MiniCover.dll"

// Needed because we're ref'ing the dll from the build
using Cake.MiniCover;
using Cake.MiniCover.Settings;

class FailTheBuildException : Exception
{
    public FailTheBuildException() : base("The build should have failed")
    {
    }
}

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

const string SOLUTION = "./Sample.sln";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Test::ThrowsForMissingToolsProject")
    .Does(() => 
{
    try
    {
        MiniCoverReset();
    }
    catch (InvalidOperationException)
    {
    }
});

Task("Test::Prepare")
    .Does(() =>
{
    DotNetCoreBuild(SOLUTION, new DotNetCoreBuildSettings
    {
        Configuration = configuration
    });

    SetMiniCoverToolsProject("./minicover/minicover.csproj");
});

Task("Test::Composite")
    .IsDependentOn("Test::Prepare")
    .Does(() => 
{
    MiniCover(tool =>
        tool.DotNetCoreTest("./test/Sample.MyLib.Tests/Sample.MyLib.Tests.csproj", new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true
        }),
        new MiniCoverSettings()
            .WithAssembliesMatching("./test/**/*.dll")
            .WithSourcesMatching("./src/**/*.cs")
            .GenerateReport(ReportType.CONSOLE | ReportType.XML)
            .WithNonFatalThreshold()
    );
});

Task("Test::Individual")
   .IsDependentOn("Test::Prepare")
    .Does(() => 
{
    var settings = new MiniCoverSettings()
        .WithAssembliesMatching("./test/**/*.dll")
        .WithSourcesMatching("./src/**/*.cs")
        .GenerateReport(ReportType.CONSOLE | ReportType.XML)
        .WithNonFatalThreshold();
    
    MiniCoverInstrument(settings);
    MiniCoverReset(settings);

    DotNetCoreTest("./test/Sample.MyLib.Tests/Sample.MyLib.Tests.csproj", new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoRestore = true,
        NoBuild = true
    });

    MiniCoverUninstrument(settings);
    MiniCoverReport(settings);
});

Task("Test::FailsForFatalThreshold")
    .IsDependentOn("Test::Prepare")
    .Does(() =>
{
    try
    {
        MiniCover(tool =>
            tool.DotNetCoreTest("./test/Sample.MyLib.Tests/Sample.MyLib.Tests.csproj", new DotNetCoreTestSettings
            {
                Configuration = configuration,
                NoRestore = true,
                NoBuild = true
            }),
            new MiniCoverSettings()
                .WithAssembliesMatching("./test/**/*.dll")
                .WithSourcesMatching("./src/**/*.cs")
                .GenerateReport(ReportType.CONSOLE | ReportType.XML)
                .WithFatalThreshold()
        );
        throw new FailTheBuildException();
    }
    catch(Exception ex)
    {
        if (ex is FailTheBuildException)
        {
            throw;
        }
    }
});

Task("Test")
    .IsDependentOn("Test::ThrowsForMissingToolsProject")
    .IsDependentOn("Test::Composite")
    .IsDependentOn("Test::Individual")
    .IsDependentOn("Test::FailsForFatalThreshold");

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);