#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void StickyAdv()
        {
            string copyCode = FileTextCopy("StickyAdv_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif