using System;
using System.Collections.Generic;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.MiniCover.Settings;

namespace Cake.MiniCover
{
    public static partial class MiniCoverAliases
    {
        private static readonly Dictionary<ReportType, Action<MiniCoverSettings,ProcessArgumentBuilder>> AdditionalArguments = new Dictionary<ReportType, Action<MiniCoverSettings,ProcessArgumentBuilder>>
        {
            { 
                ReportType.COVERALLS, 
                (s, a) => 
                {
                    
                }
            }
        };

        /// <summary>
        /// Generate one or more minicover reports
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void MiniCoverReport(this ICakeContext ctx, MiniCoverSettings settings)
        {
            if (ctx == null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }
            
            ctx.EnsureToolsProjectLocated();

            foreach(Enum flag in Enum.GetValues(typeof(ReportType)))
            {
                if(settings.ReportType.HasFlag(flag))
                {
                    var subcommand = typeof(ReportType).GetMember(flag.ToString())[0].GetCustomAttributes(typeof(ReportCommandAttribute), false)[0] as ReportCommandAttribute;

                    ctx.MiniCoverReport(settings, subcommand.CommandName, a => 
                    {
                        a.AppendReportOutput(settings, subcommand.OutputName ?? string.Empty);

                        if (subcommand.SupportsThreshold)
                        {
                            a.Append("--threshold");
                            a.Append(settings.FailureThreshold.ToString("0.00"));
                        }

                        if(AdditionalArguments.TryGetValue((ReportType) flag, out var additionalArgs))
                        {
                            additionalArgs.Invoke(settings, a);
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Generate a report using the specified report provider name. Only use this if
        /// if you need to generate a report for which Cake.MiniCover does not yet have
        /// settings for.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="reportName">The report provider to use</param>
        [CakeMethodAlias]
        public static void MiniCoverReport(
            this ICakeContext ctx,
            MiniCoverSettings settings,
            string reportName
        )
        {
            if (ctx == null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }
            
            ctx.EnsureToolsProjectLocated();
            ctx.MiniCoverReport(settings, reportName, null);
        }
        
        /// <summary>
        /// Generate a report using the specified report provider name. Only use this if
        /// if you need to generate a report for which Cake.MiniCover does not yet have
        /// settings for.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="reportName">The report provider to use</param>
        /// <param name="additionalArgs">Any additional arguments to specify</param>
        [CakeMethodAlias]
        public static void MiniCoverReport(
            this ICakeContext ctx,
            MiniCoverSettings settings,
            string reportName,
            Action<ProcessArgumentBuilder> additionalArgs
        )
        {
            if (ctx == null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }
            
            ctx.EnsureToolsProjectLocated();
            
            var args = new ProcessArgumentBuilder().AppendReportCommand(reportName, settings);
            
            additionalArgs?.Invoke(args);
            
            try
            {
                ctx.DotNetCoreTool(MiniCoverSettings.MiniCoverToolsProject, "minicover", args);
            }
            catch(Exception ex) when (settings.NonFatalThreshold)
            {
                // TODO: Better detection of when failure is actually due to coverage threshold
                ctx.Debug("Report generation failed silently: {0}", ex);
            }
        }
    }
}