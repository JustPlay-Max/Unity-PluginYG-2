#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void Bonuses()
        {
            string copyCode = FileTextCopy("Bonuses_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif