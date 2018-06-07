//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

const string SOLUTION = "./Cake.MiniCover.sln";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

IEnumerable<string> gitOutput;
if (StartProcess("git", new ProcessSettings { 
        Arguments = "rev-parse --abbrev-ref HEAD",
        RedirectStandardOutput = true
    },
    out gitOutput
) != 0)
{
    throw new Exception("Failed to get the git branch");
}

var branch = TravisCI.IsRunningOnTravisCI ? 
    TravisCI.Environment.Build.Branch : gitOutput.FirstOrDefault();


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean::Dist")
    .Does(() => 
{
    if(DirectoryExists("./_dist"))
    {
        DeleteDirectory("./_dist", new DeleteDirectorySettings
        {
            Recursive = true
        });
    }
});

Task("Clean::Test")
    .Does(() => 
{
    if(DirectoryExists("./test/_addin"))
    {
        DeleteDirectory("./test/_addin", new DeleteDirectorySettings
        {
            Recursive = true
        });
    }
});

Task("Clean")
    .IsDependentOn("Clean::Dist")
    .IsDependentOn("Clean::Test")
    .Does(() =>
{
    DotNetCoreClean(SOLUTION);
});

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore(SOLUTION);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreBuild(SOLUTION, new DotNetCoreBuildSettings {
        Configuration = configuration
    });
});

Task("Test")
    .IsDependentOn("Clean::Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists("./test/_addin");
    CopyFiles($"./src/Cake.MiniCover/bin/{configuration}/netstandard2.0/Cake.MiniCover.*", "./test/_addin");
    CakeExecuteScript("./test/integration-tests.cake", new CakeSettings
    {
        Verbosity = Verbosity.Diagnostic
    });
});

Task("Dist")
    .IsDependentOn("Clean::Dist")
    .IsDependentOn("Test")
    .Does(() => 
{
    var version = System.IO.File.ReadAllText(new FilePath("./version.txt").FullPath).Trim();

    if(branch == "next")
    {
        version += $"-next{DateTime.Now.ToString("yyyMMddhhmmss")}";
    }

    EnsureDirectoryExists("./_dist");
    DotNetCorePack("./src/Cake.MiniCover/Cake.MiniCover.csproj", new DotNetCorePackSettings
    {
        Configuration = configuration,
        NoRestore = true,
        OutputDirectory = "./_dist",
        ArgumentCustomization = args => args.AppendQuoted($"/p:PackageVersion={version}")
    });
});

Task("Publish")
    .IsDependentOn("Dist")
    .Does(() => 
{
    if (branch != "master" && branch != "next")
    {
        Warning($"Not publishing on branch '{branch}'");
        return;
    }

    var apiKey = Argument<string>("NugetApiKey");
    var feed = Argument("NugetFeed", "https://api.nuget.org/v3/index.json");

    foreach(var nupkg in GetFiles("./_dist/Cake.MiniCover*.nupkg"))
    {
        if (nupkg.FullPath.EndsWith(".symbols.nupkg"))
        {
            continue;
        }

        DotNetCoreNuGetPush(nupkg.FullPath, new DotNetCoreNuGetPushSettings
        {
            Source = feed,
            ApiKey = apiKey
        });
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);