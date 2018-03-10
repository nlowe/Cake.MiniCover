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
        /// Reset coverage for the minicover project identified by the provided settings
        /// </summary>
        /// <param name="ctx">The context.</param>
        [CakeMethodAlias]
        public static void MiniCoverReset(this ICakeContext ctx) =>
            ctx.MiniCoverReset(new MiniCoverSettings());
        
        /// <summary>
        /// Reset coverage for the minicover project identified by the provided settings
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void MiniCoverReset(
            this ICakeContext ctx,
            MiniCoverSettings settings
        )
        {
            ctx.EnsureToolsProjectLocated();
            ctx.DotNetCoreTool(
                MiniCoverSettings.MiniCoverToolsProject,
                "minicover", 
                new ProcessArgumentBuilder().AppendCommonArgs("reset", settings)
            );
        }
    }
}