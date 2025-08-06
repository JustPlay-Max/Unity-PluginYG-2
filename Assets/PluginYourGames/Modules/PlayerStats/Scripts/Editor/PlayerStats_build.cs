#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void PlayerStats()
        {
            InitFunction("GetStats");

            string copyCode = FileTextCopy("PlayerStats_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif