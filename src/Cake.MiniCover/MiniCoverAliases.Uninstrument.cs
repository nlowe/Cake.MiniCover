using System;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.MiniCover.Settings;

namespace Cake.MiniCover
{
    public static partial class MiniCoverAliases
    {
        /// <summary>
        /// Uninstrument the assemblies that were instrumented for code coverage
        /// </summary>
        /// <param name="ctx">The context.</param>
        [CakeMethodAlias]
        public static void MiniCoverUninstrument(this ICakeContext ctx)
        {
            if (ctx == null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }
            
            ctx.MiniCoverUninstrument(new MiniCoverSettings());
        }
        
        /// <summary>
        /// Uninstrument the assemblies that were instrumented for code coverage
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void MiniCoverUninstrument(
            this ICakeContext ctx,
            MiniCoverSettings settings
        )
        {
            ctx.EnsureToolsProjectLocated();
            ctx.DotNetCoreTool(
                MiniCoverSettings.MiniCoverToolsProject,
                "minicover", 
                new ProcessArgumentBuilder().AppendMiniCoverCommand("uninstrument", settings)
            );
        }
    }
}