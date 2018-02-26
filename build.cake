#addin "nuget:?package=Cake.Codecov&version=0.3.0"
#tool "nuget:?package=Codecov&version=1.0.3"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

const string SOLUTION = "./Cake.Minicover.sln";

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
    if(DirectoryExists("_tests"))
    {
        DeleteDirectory("_tests", new DeleteDirectorySettings
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
    .IsDependentOn("Bundle")
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
    foreach(var project in GetFiles("./test/**/*.csproj"))
    {
        TestProject(project);
    }
});

Task("Dist")
    .IsDependentOn("Clean::Dist")
    .IsDependentOn("Test")
    .Does(() => 
{
});

Task("CodeCov::Publish")
    .IsDependentOn("Test")
    .Does(() =>
{
    Codecov("./_tests/coverage.xml");
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