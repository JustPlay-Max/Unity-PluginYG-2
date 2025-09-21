namespace YG.EditorScr.BuildModify
{
    using UnityEditor.Build.Reporting;
    using UnityEditor.Build;
    using System.IO;

    public class ProcessBuild : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public static string BuildPath { get; private set; } = string.Empty;
        public int callbackOrder => -1000;
        public void OnPreprocessBuild(BuildReport report)
        {
            int.TryParse(BuildLog.ReadProperty("Build number"), out int buildNumInt);
            buildNumInt += 1;
            YG2.infoYG.Basic.buildNumber = buildNumInt;

            BuildPath = report.summary.outputPath;
#if PLATFORM_WEBGL
            if (!string.IsNullOrEmpty(BuildPath))
            {
                DeleteIfFileExist($"{BuildPath}/index.html");
                DeleteIfFileExist($"{BuildPath}/style.css");
            }
#endif
            if (YG2.infoYG.Basic.platform != null && YG2.infoYG.Basic.autoApplySettings)
                InfoYG.Inst().Basic.platform.ApplyProjectSettings();
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            ModifyBuild.ModifyIndex();

            if (YG2.infoYG.Basic.archivingBuild)
                ArchivingBuild.Archiving(BuildPath);

            BuildLog.WritingLog();
        }

#if PLATFORM_WEBGL
        private void DeleteIfFileExist(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
#endif
    }
}