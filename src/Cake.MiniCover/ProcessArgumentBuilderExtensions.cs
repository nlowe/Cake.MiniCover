using Cake.Core;
using Cake.Core.IO;
using Cake.MiniCover.Settings;

namespace Cake.MiniCover
{
    internal static class ProcessArgumentBuilderExtensions
    {
        internal static ProcessArgumentBuilder AppendCommonArgs(this ProcessArgumentBuilder builder, string subcommand,
            MiniCoverSettings settings)
        {
            builder.Append(subcommand);

            if (settings.MiniCoverWorkingDirectory != null)
            {
                builder.Append("--workdir");
                builder.AppendQuoted(settings.MiniCoverWorkingDirectory.FullPath);
            }
            
            if (!string.IsNullOrEmpty(settings.CoverageFileName))
            {
                builder.Append("--coverage-file");
                builder.AppendQuoted(settings.CoverageFileName);
            }

            return builder;
        }
        
        internal static ProcessArgumentBuilder AppendGlobPatterns(this ProcessArgumentBuilder builder,
            MiniCoverSettings settings)
        {
            foreach (var glob in settings.AssemblyIncludePatterns)
            {
                builder.Append("--assemblies");
                builder.AppendQuoted(glob);
            }

            foreach (var glob in settings.AssemblyExcludeGlobPatterns)
            {
                builder.Append("--exclude-assemblies");
                builder.AppendQuoted(glob);
            }

            foreach (var glob in settings.SourcesGlobPatterns)
            {
                builder.Append("--sources");
                builder.AppendQuoted(glob);
            }

            foreach (var glob in settings.SourcesExcludeGlobPatterns)
            {
                builder.Append("--exclude-sources");
                builder.AppendQuoted(glob);
            }
            
            return builder;
        }

        internal static ProcessArgumentBuilder AppendReportOutput(this ProcessArgumentBuilder builder,
            MiniCoverSettings settings, string suffix = "")
        {
            if (!string.IsNullOrEmpty(settings?.ReportPrefix))
            {
                builder.Append("--output");
                builder.AppendQuoted($"{settings.ReportPrefix}{suffix}");
            }

            return builder;
        }
    }
}