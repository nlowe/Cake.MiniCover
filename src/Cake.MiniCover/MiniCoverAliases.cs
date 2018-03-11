using System;
using System.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.MiniCover.Settings;

namespace Cake.MiniCover
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/lucaslorentz/minicover">MiniCover</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, create a tools project and use the
    /// <see cref="SetMiniCoverToolsProject"/> alias.
    /// </para>
    /// </summary>
    /// <example>
    /// <code>
    /// SetMiniCoverToolsProject("./minicover/minicover.csproj");
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
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// SetMiniCoverToolsProject("./minicover/minicover.csproj");
    /// MiniCoverInstrument(
    ///     new MiniCoverSettings()
    ///         .WithAssembliesMatching("./test/**/*.dll")
    ///         .WithSourcesMatching("./src/**/*.cs")
    /// );
    /// MiniCoverReset();
    /// DotNetCoreTest(...);
    /// MiniCoverUninstrument();
    /// MiniCoverReport(new MiniCoverSettings().GenerateReport(ReportType.CONSOLE | ReportType.XML));
    /// </code>
    /// </example>
    [CakeAliasCategory("MiniCover")]
    [CakeNamespaceImport("Cake.MiniCover")]
    [CakeNamespaceImport("Cake.MiniCover.Settings")]
    public static partial class MiniCoverAliases
    {
        private static void EnsureToolsProjectLocated(this ICakeContext ctx)
        {
            if (string.IsNullOrEmpty(MiniCoverSettings.MiniCoverToolsProject))
            {
                throw new InvalidOperationException("The MiniCover tools helper project has not yet been located. Please call SetMiniCoverToolsProject(...) first.");
            }
        }
        
        /// <summary>
        /// Set the location of the 'csproj' that contains the MiniCover DotNetCliToolReference
        /// and restore packages for it
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="miniCoverHelperProject">The path to the csproj to use for MiniCover.</param>
        [CakeMethodAlias]
        public static void SetMiniCoverToolsProject(this ICakeContext ctx, FilePath miniCoverHelperProject)
        {
            if (ctx == null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }

            var path = miniCoverHelperProject?.FullPath ?? throw new ArgumentNullException(nameof(miniCoverHelperProject));
            if (!File.Exists(path))
            {
                throw new ArgumentException("Could not find minicover tools project at the specified location", nameof(miniCoverHelperProject));
            }

            MiniCoverSettings.MiniCoverToolsProject = path;
            ctx.DotNetCoreRestore(path);
        }
        
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

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            
            ctx.EnsureToolsProjectLocated();
            
            ctx.MiniCoverInstrument(settings);
            ctx.MiniCoverReset(settings);

            action.Invoke(ctx);
            
            ctx.MiniCoverUninstrument(settings);
            ctx.MiniCoverReport(settings);
        }
    }
}