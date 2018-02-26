using System;

namespace Cake.Minicover
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
        CONSOLE = 1,
        /// <summary>
        /// Generate an HTML Coverage Report
        /// </summary>
        HTML = 2,
        /// <summary>
        /// Generate an NCover Compatable XML Coverage Report
        /// </summary>
        XML = 4
    }
}