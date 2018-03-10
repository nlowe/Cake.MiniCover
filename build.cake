//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

const string SOLUTION = "./Cake.MiniCover.sln";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

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
    EnsureDirectoryExists("./_dist");
    DotNetCorePack("./src/Cake.MiniCover/Cake.MiniCover.csproj", new DotNetCorePackSettings
    {
        Configuration = configuration,
        NoRestore = true,
        OutputDirectory = "./_dist"
    });
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