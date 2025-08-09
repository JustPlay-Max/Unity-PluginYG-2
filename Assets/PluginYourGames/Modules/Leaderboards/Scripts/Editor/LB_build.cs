#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void Leaderboards()
        {
            string copyCode = FileTextCopy("LB_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif