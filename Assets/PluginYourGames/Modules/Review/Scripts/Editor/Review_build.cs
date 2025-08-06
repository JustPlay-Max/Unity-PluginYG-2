#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void Review()
        {
            InitFunction("InitReview");

            string copyCode = FileTextCopy("Review_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif