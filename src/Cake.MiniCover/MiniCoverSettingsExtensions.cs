using System;

namespace Cake.Minicover
{
    /// <summary>
    /// Contains extensions for <see cref="MiniCoverSettings"/>.
    /// </summary>
    public static class MiniCoverSettingsExtensions
    {
        /// <summary>
        /// Instrument test assemblies matching the specified pattern
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="pattern">The glob pattern to use when searching for test assemblies</param>
        /// <returns>The <see cref="MiniCoverSettings"/> instance so that multiple calls can be chained</returns>
        public static MiniCoverSettings WithAssembliesMatching(this MiniCoverSettings settings, string pattern)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.AssemblyIncludePatterns.Add(pattern);
            
            return settings;
        }

        /// <summary>
        /// Exclude from instrumentation all test assemblies matching the specified pattern
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="pattern">The glob pattern to use when searching for test assemblies to exclude</param>
        /// <returns>The <see cref="MiniCoverSettings"/> instance so that multiple calls can be chained</returns>
        public static MiniCoverSettings WithoutAssebliesMatching(this MiniCoverSettings settings, string pattern)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.AssemblyExcludeGlobPatterns.Add(pattern);
            
            return settings;
        }
        
        /// <summary>
        /// Measure coverage on files matching the specified pattern
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="pattern">The glob pattern to use when searching for source files</param>
        /// <returns>The <see cref="MiniCoverSettings"/> instance so that multiple calls can be chained</returns>
        public static MiniCoverSettings WithSourcesMatching(this MiniCoverSettings settings, string pattern)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.SourcesGlobPatterns.Add(pattern);
            
            return settings;
        }
        
        /// <summary>
        /// Exclude files matching the specified pattern from coverage results
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="pattern">The glob pattern to use when searching for source files</param>
        /// <returns>The <see cref="MiniCoverSettings"/> instance so that multiple calls can be chained</returns>
        public static MiniCoverSettings WithoutSourcesMatching(this MiniCoverSettings settings, string pattern)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.SourcesExcludeGlobPatterns.Add(pattern);
            
            return settings;
        }

        /// <summary>
        /// Use the provided name for the hits file
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="hitsFile">The name of the hits file</param>
        /// <returns>The <see cref="MiniCoverSettings"/> instance so that multiple calls can be chained</returns>
        public static MiniCoverSettings WithHitsFile(this MiniCoverSettings settings, string hitsFile)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.HitsFileName = hitsFile;
            
            return settings;    
        }
        
        /// <summary>
        /// Use the provided name for the internal coverage file
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="coverageFile">The name of the internal coverage file</param>
        /// <returns>The <see cref="MiniCoverSettings"/> instance so that multiple calls can be chained</returns>
        public static MiniCoverSettings WithCoverageFile(this MiniCoverSettings settings, string coverageFile)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.CoverageFileName = coverageFile;
            
            return settings;    
        }
        
        /// <summary>
        /// Set the coverage percentage below which the build will fail
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="threshold">The failure threshold to use</param>
        /// <returns>The <see cref="MiniCoverSettings"/> instance so that multiple calls can be chained</returns>
        public static MiniCoverSettings WithThreshold(this MiniCoverSettings settings, float threshold)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.FailureThreshold = threshold;
            
            return settings;    
        }

        /// <summary>
        /// Set the report type to generate
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="reportType">The report type or types to generate</param>
        /// <returns>The <see cref="MiniCoverSettings"/> instance so that multiple calls can be chained</returns>
        /// <example>
        /// <code>
        /// // Print results to the console and generate an XML report
        /// var settings = new MiniCoverSettings().GenerateReport(ReportType.CONSOLE | ReportType.XML);
        /// </code>
        /// </example>
        public static MiniCoverSettings GenerateReport(this MiniCoverSettings settings, ReportType reportType)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.ReportType = reportType;
            
            return settings; 
        }

        /// <summary>
        /// Use the specified prefix for report generation
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="prefix">The report prefix</param>
        /// <returns>The <see cref="MiniCoverSettings"/> instance so that multiple calls can be chained</returns>
        /// <example>
        /// // Genarate an html report in myCoverage-html/ and an xml report in myCoverage.xml
        /// var settings = new MiniCoverSettings().GenerateReport(ReportType.HTML | ReportType.XML).WithReportPrefix("myCoverage");
        /// </example>
        public static MiniCoverSettings WithReportPrefix(this MiniCoverSettings settings, string prefix)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.ReportPrefix = prefix;

            return settings;
        }
    }
}