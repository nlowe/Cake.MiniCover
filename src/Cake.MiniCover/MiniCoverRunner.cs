using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Minicover
{
    public class MiniCoverRunner : DotNetCoreTool<MiniCoverSettings>
    {
        private readonly ICakeEnvironment _environment;

        public MiniCoverRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools
        ) : base(fileSystem, environment, processRunner, tools) =>
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));

        public void Run(ICakeContext ctx, Action<ICakeContext> testAction, MiniCoverSettings settings)
        {
            RunCommand(settings, InstrumentTestAssemblies(settings));
            RunCommand(settings, ResetCoverage(settings));
            testAction.Invoke(ctx);
            RunCommand(settings, UninstrumentTestAssemblies(settings));
            GenerateReports(settings);
        }

        private ProcessArgumentBuilder InstrumentTestAssemblies(MiniCoverSettings settings)
        {
            var builder = CreateCommonArgs("instrument", settings);
            builder.AppendGlobPatterns(settings);

            if (!string.IsNullOrEmpty(settings.HitsFileName))
            {
                builder.Append("--hits-file");
                builder.AppendQuoted(settings.HitsFileName);
            }

            if (!string.IsNullOrEmpty(settings.CoverageFileName))
            {
                builder.Append("--coverage-file");
                builder.AppendQuoted(settings.CoverageFileName);
            }
            
            return builder;
        }
        
        private ProcessArgumentBuilder ResetCoverage(MiniCoverSettings settings)
        {
            var builder = CreateCommonArgs("reset", settings);
            
            if (!string.IsNullOrEmpty(settings.CoverageFileName))
            {
                builder.Append("--coverage-file");
                builder.AppendQuoted(settings.CoverageFileName);
            }
            
            return builder;
        }
        
        private ProcessArgumentBuilder UninstrumentTestAssemblies(MiniCoverSettings settings)
        {
            var builder = CreateCommonArgs("uninstrument", settings);
            
            if (!string.IsNullOrEmpty(settings.CoverageFileName))
            {
                builder.Append("--coverage-file");
                builder.AppendQuoted(settings.CoverageFileName);
            }

            return builder;
        }

        private void GenerateReports(MiniCoverSettings settings)
        {
            if (settings.ReportType.HasFlag(ReportType.CONSOLE))
            {
                RunCommand(settings, GenerateConsoleReport(settings));
            }

            if (settings.ReportType.HasFlag(ReportType.HTML))
            {
                RunCommand(settings, GenerateHtmlReport(settings));
            }

            if (settings.ReportType.HasFlag(ReportType.XML))
            {
                RunCommand(settings, GenerateXmlReport(settings));
            }
        }

        private ProcessArgumentBuilder GenerateConsoleReport(MiniCoverSettings settings)
        {
            var builder = CreateCommonArgs("report", settings);

            if (!string.IsNullOrEmpty(settings.CoverageFileName))
            {
                builder.Append("--coverage-file");
                builder.AppendQuoted(settings.CoverageFileName);
            }

            var threshold = Math.Min(Math.Max(settings.FailureThreshold, 0.0f), 100.0f);
            builder.Append("--threshold");
            builder.AppendQuoted(threshold.ToString("0.00"));
            
            return builder;
        }

        private ProcessArgumentBuilder GenerateHtmlReport(MiniCoverSettings settings)
        {
            var builder = CreateCommonArgs("htmlreport", settings);

            if (!string.IsNullOrEmpty(settings.CoverageFileName))
            {
                builder.Append("--coverage-file");
                builder.AppendQuoted(settings.CoverageFileName);
            }

            var threshold = Math.Min(Math.Max(settings.FailureThreshold, 0.0f), 100.0f);
            builder.Append("--threshold");
            builder.AppendQuoted(threshold.ToString("0.00"));

            if (!string.IsNullOrEmpty(settings.ReportPrefix))
            {
                builder.Append("--output");
                builder.AppendQuoted($"{settings.ReportPrefix}-html");
            }
            
            return builder;
        }

        private ProcessArgumentBuilder GenerateXmlReport(MiniCoverSettings settings)
        {
            var builder = CreateCommonArgs("xmlreport", settings);

            if (!string.IsNullOrEmpty(settings.CoverageFileName))
            {
                builder.Append("--coverage-file");
                builder.AppendQuoted(settings.CoverageFileName);
            }

            var threshold = Math.Min(Math.Max(settings.FailureThreshold, 0.0f), 100.0f);
            builder.Append("--threshold");
            builder.AppendQuoted(threshold.ToString("0.00"));
            
            if (!string.IsNullOrEmpty(settings.ReportPrefix))
            {
                builder.Append("--output");
                builder.AppendQuoted($"{settings.ReportPrefix}.xml");
            }
            
            return builder;
        }

        private ProcessArgumentBuilder CreateCommonArgs(string subcommand, MiniCoverSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);
            builder.Append("minicover");
            builder.Append(subcommand);
            
            if (settings.WorkingDirectory != null)
            {
                builder.Append("--workdir");
                builder.AppendQuoted(settings.WorkingDirectory.FullPath);
            }

            return builder;
        }
        
        protected override string GetToolName() => ".NET Core CLI";

        protected override IEnumerable<string> GetToolExecutableNames() => new[]
        {
            "dotnet", "dotnet.exe"
        };
    }
}