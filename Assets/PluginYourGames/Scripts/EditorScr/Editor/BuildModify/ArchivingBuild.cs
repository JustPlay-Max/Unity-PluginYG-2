using System.IO;
using System.IO.Compression;
using YG.Insides;

namespace YG.EditorScr.BuildModify
{
    public static class ArchivingBuild
    {
        public static void Archiving(string pathToBuiltProject)
        {
#if PLATFORM_WEBGL
            var buildName = pathToBuiltProject;

            buildName += $"_{PlatformSettings.currentPlatformBaseName}";

            if (int.TryParse(BuildLog.ReadProperty("Build number"), out int buildNumber))
            {
                buildNumber++;
                buildName += $"_Build({buildNumber})";
            }

            buildName += ".zip";

            if (File.Exists(buildName))
                File.Delete(buildName);

            ZipFile.CreateFromDirectory(
                pathToBuiltProject,
                buildName,
                CompressionLevel.Optimal,
                false
            );
#endif
        }
    }
}