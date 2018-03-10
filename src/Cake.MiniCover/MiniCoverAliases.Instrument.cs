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
        /// Instrument the specified assemblies to gather coverage data
        /// </summary>
        /// <param name="ctx">The context.</param>
        [CakeMethodAlias]
        public static void MiniCoverInstrument(this ICakeContext ctx) =>
            ctx.MiniCoverInstrument(new MiniCoverSettings());
        
        /// <summary>
        /// Instrument the specified assemblies to gather coverage data
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void MiniCoverInstrument(this ICakeContext ctx, MiniCoverSettings settings)
        {
            if (ctx == null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }
            
            ctx.EnsureToolsProjectLocated();
            
            var args = new ProcessArgumentBuilder().AppendCommonArgs("instrument", settings);
                
            args.AppendGlobPatterns(settings);

            if (!string.IsNullOrEmpty(settings.HitsFileName))
            {
                args.Append("--hits-file");
                args.AppendQuoted(settings.HitsFileName);
            }
            
            ctx.DotNetCoreTool(MiniCoverSettings.MiniCoverToolsProject, "minicover", args);
        }
    }
}