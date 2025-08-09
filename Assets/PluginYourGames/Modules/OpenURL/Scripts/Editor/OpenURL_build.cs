#if YandexGamesPlatform_yg

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void OpenURL()
        {
            InitFunction("GetAllGames");

            string copyCode = FileTextCopy("OpenURL_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif
