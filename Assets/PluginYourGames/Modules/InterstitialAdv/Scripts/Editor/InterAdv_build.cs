#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void InterstitialAdv()
        {
            //if (infoYG.InterstitialAdv.showFirstAdv)
            //{
            //    string addInitCode = "InterAdvShow();";
            //    AddIndexCode(addInitCode, CodeType.Init0);

            //    string copyCodeAddStartCode = FileTextCopy("InterAdv_start.js");
            //    AddIndexCode(copyCodeAddStartCode, CodeType.Start);
            //}

            string copyCode = FileTextCopy("InterAdv_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif