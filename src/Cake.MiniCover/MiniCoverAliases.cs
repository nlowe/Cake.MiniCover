using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Minicover
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/lucaslorentz/minicover">MiniCover</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from Nuget.org, or specify the ToolPath within the <see cref="MiniCoverSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=MiniCover&version=2.0.0-ci-20180214203020"
    /// </code>
    /// </para>
    /// </summary>
    /// <example>
    /// MiniCover(tool =>
    ///     {
    ///         foreach(var project in GetFiles("./test/**/*.csproj"))
    ///         {
    ///             tool.DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings()
    ///             {
    ///                 // Required to keep instrumentation added by MiniCover
    ///                 NoBuild = true,
    ///                 Configuration = configuration
    ///             });
    ///         }
    ///     },
    ///     new MiniCoverSettings()
    ///         .WithAssembliesMatching("./test/**/*.dll")
    ///         .WithSourcesMatching("./src/**/*.cs")
    ///         .GenerateReport(ReportType.CONSOLE | ReportType.XML)
    /// );
    /// </example>
    [CakeAliasCategory("MiniCover")]
    public static class MiniCoverAliases
    {
        /// <summary>
        /// Instruments test assemblies using <see href="https://github.com/lucaslorentz/minicover">MiniCover</see>
        /// before executing the specified test action to generate code coverage.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="action">The test action to perform.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void MiniCover(this ICakeContext ctx, Action<ICakeContext> action, MiniCoverSettings settings)
        {
            if (ctx == null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }
            
            var runner = new MiniCoverRunner(ctx.FileSystem, ctx.Environment, ctx.ProcessRunner, ctx.Tools);
            
            runner.Run(ctx, action, settings);
        }
    }
}