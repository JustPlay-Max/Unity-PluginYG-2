#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void GameLabel()
        {
            InitFunction("InitGameLabel");

            string copyCode = FileTextCopy("GameLabel_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif