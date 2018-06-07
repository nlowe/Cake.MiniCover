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
            
            if (settings.ReportType.HasFlag(ReportType.CONSOLE))
            {
                ctx.MiniCoverReport(settings, "report");
            }

            if (settings.ReportType.HasFlag(ReportType.HTML))
            {
                ctx.MiniCoverReport(settings, "htmlreport", a => a.AppendReportOutput(settings));
            }

            if (settings.ReportType.HasFlag(ReportType.XML))
            {
                var suffix = settings.ReportType.HasFlag(ReportType.OPENCOVER) ? "-ncover.xml" : ".xml"; 
                
                ctx.MiniCoverReport(settings, "xmlreport", a => a.AppendReportOutput(settings, suffix));
            }

            if (settings.ReportType.HasFlag(ReportType.OPENCOVER))
            {
                var suffix = settings.ReportType.HasFlag(ReportType.OPENCOVER) ? "-opencover.xml" : ".xml"; 

                ctx.MiniCoverReport(settings, "opencoverreport", a => a.AppendReportOutput(settings, suffix));
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

            args.Append("--threshold");
            args.Append(settings.FailureThreshold.ToString("0.00"));
            
            additionalArgs?.Invoke(args);
            
            try
            {
                ctx.DotNetCoreTool(MiniCoverSettings.MiniCoverToolsProject, "minicover", args);
            }
            catch(Exception)
            {
                if(!settings.NonFatalThreshold)
                {
                    throw;
                }
            }
        }
    }
}