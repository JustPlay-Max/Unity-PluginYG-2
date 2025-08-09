#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void TV()
        {
            AddIndexCode("TVInit();", CodeType.Init);

            string copyCode = FileTextCopy("TV_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif