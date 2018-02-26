using Cake.Core;
using Cake.Core.IO;

namespace Cake.Minicover
{
    internal static class ProcessArgumentBuilderExtensions
    {
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
    }
}