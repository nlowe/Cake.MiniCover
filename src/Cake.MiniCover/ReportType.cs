using System;

namespace Cake.MiniCover
{
    /// <summary>
    /// The type of report to generate
    /// </summary>
    [Flags]
    public enum ReportType
    {
        /// <summary>
        /// Print coverate results to the console
        /// </summary>
        CONSOLE = 1 << 0,
        /// <summary>
        /// Generate an HTML Coverage Report
        /// </summary>
        HTML = 1 << 1,
        /// <summary>
        /// Generate an NCover Compatable XML Coverage Report
        /// </summary>
        XML = 1 << 2,
        /// <summary>
        /// Generate an OpenCover Compatable XML Coverage Report
        /// </summary>
        OPENCOVER = 1 << 3,
        
    }
}