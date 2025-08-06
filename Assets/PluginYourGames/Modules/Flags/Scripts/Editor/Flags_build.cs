#if YandexGamesPlatform_yg

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void Flags()
        {
            InitFunction("GetFlags");

            string copyCode = FileTextCopy("Flags_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif
