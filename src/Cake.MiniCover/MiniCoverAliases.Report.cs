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
                    if(!string.IsNullOrEmpty(s.Coveralls?.RootPath))
                    {
                        a.Append("--root-path").AppendQuoted(s.Coveralls.RootPath);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.ServiceJobId))
                    {
                        a.Append("--service-job-id").AppendQuoted(s.Coveralls.ServiceJobId);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.ServiceName))
                    {
                        a.Append("--service-name").AppendQuoted(s.Coveralls.ServiceName);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.RepoToken))
                    {
                        a.Append("--repo-token").AppendQuotedSecret(s.Coveralls.RepoToken);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.CommitHash))
                    {
                        a.Append("--commit").AppendQuoted(s.Coveralls.CommitHash);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.CommitMessage))
                    {
                        a.Append("--commit-message").AppendQuoted(s.Coveralls.CommitMessage);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.CommitAuthorName))
                    {
                        a.Append("--commit-author-name").AppendQuoted(s.Coveralls.CommitAuthorName);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.CommitAuthorEmail))
                    {
                        a.Append("--commit-author-email").AppendQuoted(s.Coveralls.CommitAuthorEmail);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.CommitterName))
                    {
                        a.Append("--commit-committer-name").AppendQuoted(s.Coveralls.CommitterName);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.CommitterEmail))
                    {
                        a.Append("--commit-committer-email").AppendQuoted(s.Coveralls.CommitterEmail);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.Branch))
                    {
                        a.Append("--branch").AppendQuoted(s.Coveralls.Branch);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.Remote))
                    {
                        a.Append("--remote").AppendQuoted(s.Coveralls.Remote);
                    }

                    if(!string.IsNullOrEmpty(s.Coveralls?.RemoteUrl))
                    {
                        a.Append("--remote-url").AppendQuoted(s.Coveralls.RemoteUrl);
                    }
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