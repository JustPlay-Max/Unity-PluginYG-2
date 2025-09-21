using System;
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
            var archiveFileName = $"{pathToBuiltProject}_{PlatformSettings.currentPlatformBaseName}_Build({BuildLog.GetBuildNumber() + 1})";

            if (File.Exists($"{archiveFileName}.zip"))
            {
                string dateNow = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
                string timeNow = $"{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}";
                archiveFileName += $"_{dateNow}_{timeNow}";
            }

            archiveFileName += ".zip";

            ZipFile.CreateFromDirectory(
                pathToBuiltProject,
                archiveFileName,
                CompressionLevel.Optimal,
                false
            );
#endif
        }
    }
}