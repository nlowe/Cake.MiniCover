using System;
using Cake.Core.IO;

namespace Cake.MiniCover
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class ReportCommandAttribute : Attribute
    {
        public readonly string CommandName;
        public readonly string OutputName;
        public readonly bool SupportsThreshold;

        public ReportCommandAttribute(string cmd, string outputName, bool supportsThreshold = true)
        {
            CommandName = cmd ?? throw new ArgumentNullException(nameof(cmd));
            OutputName = outputName;
            SupportsThreshold = supportsThreshold;
        }
    }

    /// <summary>
    /// The type of report to generate
    /// </summary>
    [Flags]
    public enum ReportType
    {
        /// <summary>
        /// Print coverate results to the console
        /// </summary>
        [ReportCommand("report", outputName: null)]
        CONSOLE = 1 << 0,
        /// <summary>
        /// Generate an HTML Coverage Report
        /// </summary>
        [ReportCommand("htmlreport", outputName: "coverage-htmll")]
        HTML = 1 << 1,
        /// <summary>
        /// Generate an NCover Compatable XML Coverage Report
        /// </summary>
        [ReportCommand("xmlreport", outputName: "coverage.xml")]
        XML = 1 << 2,
        /// <summary>
        /// Generate an OpenCover Compatable XML Coverage Report
        /// </summary>
        [ReportCommand("opencoverreport", outputName: "opencovercoverage.xml", supportsThreshold: false)]
        OPENCOVER = 1 << 3,
        /// <summary>
        /// Generate a Clover-formatted XML Coverage Report
        /// </summary>
        [ReportCommand("cloverreport", outputName: "clover.xml")]
        CLOVER = 1 << 4,
        /// <summary>
        /// Generate a Coveralls-formatted JSON Coverage Report
        /// </summary>
        [ReportCommand("coverallsreport", outputName: "coveralls.json")]
        COVERALLS = 1 << 5,
    }
}