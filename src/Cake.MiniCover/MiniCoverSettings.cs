using System.Collections.Generic;
using Cake.Common.Tools.DotNetCore;

namespace Cake.Minicover
{
    /// <summary>
    /// Contains settings used by <see cref="MiniCoverRunner"/>.
    /// </summary>
    public sealed class MiniCoverSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Pattern to include assemblies
        /// </summary>
        public ISet<string> AssemblyIncludePatterns { get; } = new HashSet<string>();

        /// <summary>
        /// Pattern to exclude assemblies
        /// </summary>
        public ISet<string> AssemblyExcludeGlobPatterns { get; } = new HashSet<string>();

        /// <summary>
        /// Pattern to include source files
        /// </summary>
        public ISet<string> SourcesGlobPatterns { get; } = new HashSet<string>();

        /// <summary>
        /// Pattern to exclude source files
        /// </summary>
        public ISet<string> SourcesExcludeGlobPatterns { get; } = new HashSet<string>();

        /// <summary>
        /// File name to store coverage hits in
        /// </summary>
        public string HitsFileName { get; set; } = "coverage-hits.txt";

        /// <summary>
        /// Name of json coverage file
        /// </summary>
        public string CoverageFileName { get; set; } = "coverage.json";
        
        /// <summary>
        /// The type of report or reports to generate
        /// </summary>
        public ReportType ReportType { get; set; } = ReportType.CONSOLE;

        /// <summary>
        /// The prefix for html and xml reports
        /// </summary>
        public string ReportPrefix { get; set; } = "coverage";

        /// <summary>
        /// Coverage percentage below which the build will fail
        /// </summary>
        public float FailureThreshold { get; set; } = 90.0f;
    }
}