#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void TestLabel()
        {
            if (YG2.infoYG.Templates.developerBuild)
            {
                string css = ManualFileTextCopy($"{InfoYG.CORE_FOLDER_YG2}/Platforms/YandexGames/Scripts/Editor/TestLabel/TestLabel.css");
                styleFile += $"\n\n\n{css}";

                string html = ManualFileTextCopy($"{InfoYG.CORE_FOLDER_YG2}/Platforms/YandexGames/Scripts/Editor/TestLabel/TestLabel.html");

                int.TryParse(BuildLog.ReadProperty("Build number"), out int buildNumInt);
                buildNumInt += 1;
                string buildNum = buildNumInt.ToString();

                html = html.Replace("___TEXT_LABEL___", $"Test build: {buildNum}");
                AddIndexCode(html, CodeType.Body);

                string js = ManualFileTextCopy($"{InfoYG.CORE_FOLDER_YG2}/Platforms/YandexGames/Scripts/Editor/TestLabel/TestLabel.js");
                AddIndexCode(js, CodeType.JS);
            }
        }
    }
}
#endif